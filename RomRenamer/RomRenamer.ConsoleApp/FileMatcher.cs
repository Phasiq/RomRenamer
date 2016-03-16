using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace RomRenamer.ConsoleApp
{
    public class FileMatcher
    {
        private readonly IUserReadWrite _userInteraction;
        private readonly string _titleName;
        private readonly List<Tuple<string, string>> _fileList;

        public FileMatcher(IUserReadWrite userInteraction, string titleName, IReadOnlyCollection<string> fileList)
        {
            _userInteraction = userInteraction;
            _titleName = titleName;
            _fileList = fileList.Select(x => new Tuple<string, string>(x, Path.GetFileNameWithoutExtension(x))).ToList();
        }

        public bool HasPerfectMatch()
        {
            return _fileList.Any(x => x.Item2.Equals(_titleName, StringComparison.InvariantCultureIgnoreCase));
        }

        public int LevenshteinDistance(string s1, string s2)
        {
            int l1 = s1.Length;
            int l2 = s2.Length;
            var d = new int[l1 + 1, l2 + 1];
            for (var i = 0; i <= l1; i++)
            {
                d[i, 0] = i;
            }
            for (var j = 0; j <= l2; j++)
            {
                d[0, j] = j;
            }

            for (var j = 1; j <= l2; j++)
            {
                for (var i = 1; i <=l1;i++)
                {
                    var sub1 = s1.Substring(i-1, 1);
                    var sub2 = s2.Substring(j-1, 1);
                    var comp = string.Compare(sub1, sub2, StringComparison.InvariantCultureIgnoreCase);
                    int cost = Math.Abs(comp);
                    var ci = d[i - 1, j] + 1;
                    var cd = d[i, j - 1] + 1;
                    var cs = d[i - 1, j - 1] + cost;
                    if (ci <= cd)
                    {
                        if (ci <= cs)
                        {
                            d[i, j] = ci;
                        }
                        else
                        {
                            d[i, j] = cs;
                        }
                    }
                    else
                    {
                        if (cd <= cs)
                        {
                            d[i, j] = cd;
                        }
                        else
                        {
                            d[i, j] = cs;
                        }
                    }
                }
            }
            return d[l1, l2];
        }

        public string GetUserDefinedMatch()
        {
            var matches = _fileList.Select(x =>
            {
                var comparisonResult = LevenshteinDistance(_titleName, x.Item2);
                return new NameComparer(comparisonResult, x.Item1, x.Item2);
            }).ToList();
            
            return matches.Any() ? SelectFilename(matches) : null;
        }

        private string SelectFilename(IList<NameComparer> fileTitles)
        {
            _userInteraction.WriteLine(Environment.NewLine +
                                       "Select the corresponding number to the file that matches or type 'q' to quit: " +
                                       Environment.NewLine + Environment.NewLine + _titleName + Environment.NewLine);
            var files = fileTitles.OrderBy(y => y.Score).Take(9).ToList();
            var x = 1;
            foreach (var file in files)
            {
                _userInteraction.WriteLine(x++ + ". " + file.TargetFileName + " | Score: " + file.Score);
            }
            _userInteraction.WriteLine(0 + ". No match found.");
                
            do
            {
                int selectionId;
                var userInput = _userInteraction.ReadKey();
                Console.WriteLine(string.Empty);
                if (char.ToLowerInvariant(userInput.KeyChar) == 'q')
                    return "exit";
                int.TryParse(userInput.KeyChar.ToString(), out selectionId);
                if (selectionId == 0)
                {
                    break;
                }
                if (selectionId > files.Count)
                {

                    _userInteraction.WriteLine(Environment.NewLine + "Invalid selection. Please try again.");
                }
                else
                {
                    return files[selectionId - 1].SourceFileName;
                }
            } while (true);
            
            return null;
        }
    }
}

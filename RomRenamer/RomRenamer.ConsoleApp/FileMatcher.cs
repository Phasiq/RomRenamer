using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;

namespace RomRenamer.ConsoleApp
{
    public class FileMatcher
    {
        private readonly IUserReadWrite _userInteraction;

        public FileMatcher(IUserReadWrite userInteraction)
        {
            _userInteraction = userInteraction;
        }

        public bool HasPerfectMatch(string fileName, IList<string> games)
        {
            return games.Any(x => x.Equals(fileName, StringComparison.InvariantCultureIgnoreCase));
        }

        public string GetUserDefinedMatch(string fileName, IList<string> gameTitles)
        {
            var fileNameLength = fileName.Length;
            for (int i = 1; i < fileNameLength; i++)
            {
                var fileNameSubstring = fileName.Substring(0, fileNameLength - i);
                var matches =
                    gameTitles.Where(s => s.StartsWith(fileNameSubstring, StringComparison.InvariantCultureIgnoreCase)).ToList();
                
                if (matches.Any())
                {
                    return SelectFilename(matches, 9);
                    
                }
            }
            return null;
        }

        private string SelectFilename(IList<string> fileTitles, int pageSize)
        {
            _userInteraction.WriteLine("Select the corresponding number to the file that matches");

            var pages = (fileTitles.Count + pageSize - 1)/pageSize;

            for (var i = 1; i <= pages; i++)
            {
                var titles = GetTitlesByPage(i, pageSize, fileTitles);
                int x = 1;
                foreach (var title in titles)
                {
                    _userInteraction.WriteLine(x++ + ". title");
                }
                if (i < pages)
                {
                    _userInteraction.WriteLine(0 + ". More titles...");
                }
                else
                {
                    _userInteraction.WriteLine("End");
                }
                
                do
                {
                    int selectionId;
                    int.TryParse(_userInteraction.ReadKey().KeyChar.ToString(), out selectionId);
                    if (selectionId == 0)
                    {
                        break;
                    }
                    if (selectionId < 1 || selectionId > titles.Count)
                    {
                        _userInteraction.WriteLine("Invalid selection. Please try again.");
                    }
                    else
                    {
                        return titles[selectionId - 1];
                    }
                } while (true);
            }
            return null;
        }

        private IList<string> GetTitlesByPage(int pageNumber, int pageSize, IList<string> fileTitles)
        {
            return fileTitles.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
        }
    }
}

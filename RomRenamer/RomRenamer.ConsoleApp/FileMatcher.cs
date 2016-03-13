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
        private readonly int _pageSize;
        private readonly string _fileName;
        private readonly IList<string> _gameList;
        private string _shortName;

        public FileMatcher(IUserReadWrite userInteraction, int pageSize, string fileName, IList<string> gameList)
        {
            _userInteraction = userInteraction;
            if (pageSize > 9 || pageSize < 1)
                throw new ArgumentException("The pageSize must be between 1 and 9. Current value is " + pageSize);
            _pageSize = pageSize;
            _fileName = fileName;
            _shortName = Path.GetFileNameWithoutExtension(_fileName);
            _gameList = gameList;
        }

        public bool HasPerfectMatch()
        {
            return _gameList.Any(x => x.Equals(Path.GetFileNameWithoutExtension(_fileName), StringComparison.InvariantCultureIgnoreCase));
        }

        public string GetUserDefinedMatch()
        {
            var fileNameLength = _shortName.Length;
            for (int i = 1; i < fileNameLength; i++)
            {
                var fileNameSubstring = _shortName.Substring(0, fileNameLength - i);
                var matches =
                    _gameList.Where(s => s.StartsWith(fileNameSubstring, StringComparison.InvariantCultureIgnoreCase)).ToList();
                
                if (matches.Any())
                {
                    return SelectFilename(matches);
                }
            }
            return null;
        }

        private string SelectFilename(IList<string> fileTitles)
        {
            _userInteraction.WriteLine("Select the corresponding number to the file that matches: " + _shortName);

            var pages = (fileTitles.Count + _pageSize - 1)/_pageSize;

            for (var i = 1; i <= pages; i++)
            {
                var titles = GetTitlesByPage(i, fileTitles);
                int x = 1;
                foreach (var title in titles)
                {
                    _userInteraction.WriteLine(x++ + ". " + title);
                }
                if (i < pages)
                {
                    _userInteraction.WriteLine(0 + ". More titles...");
                }
                else
                {
                    _userInteraction.WriteLine(0 + ". No results found.");
                }
                
                do
                {
                    int selectionId;
                    int.TryParse(_userInteraction.ReadKey().KeyChar.ToString(), out selectionId);
                    if (selectionId == 0)
                    {
                        break;
                    }
                    if (selectionId > titles.Count)
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

        private IList<string> GetTitlesByPage(int pageNumber, IList<string> fileTitles)
        {
            return fileTitles.Skip((pageNumber-1)*_pageSize).Take(_pageSize).ToList();
        }
    }
}

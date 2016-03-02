using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomRenamer.ConsoleApp
{
    public static class FileMatcher
    {
        public static IReadOnlyCollection<string> GetMatches(string fileName, IReadOnlyCollection<string> gameTitles)
        {
            var fileNameLength = fileName.Length;
            for (int i = 0; i < fileNameLength; i++)
            {
                var fileNameSubstring = fileName.Substring(0, fileNameLength - i);
                var matches =
                    gameTitles.Where(s => s.StartsWith(fileNameSubstring, StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (matches.Any())
                    return matches;
            }
            return new List<string>();
        } 
    }
}

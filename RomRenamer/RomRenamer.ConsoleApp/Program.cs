using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RomRenamer.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            // First, ask the user for the location of ROM files
            var directoryFinder = new DirectoryFinder(new UserReadWrite());
            IReadOnlyCollection<string> fileList = directoryFinder.Find();
            if (fileList == null)
            {
                return;
            }

            // Now, get the XML to compare to
            var xmlParser = new XmlParser(new UserReadWrite());
            IList<string> xmlTitles = xmlParser.FindTitles();
            if (xmlTitles == null)
            {
                return;
            }

            var totalFilesToProcess = fileList.Count;
            var totalFilesProcessed = 0;
            var successfulRenames = 0;

            foreach (var file in fileList)
            {
                var fileMatcher = new FileMatcher(new UserReadWrite(), 9);
                if (fileMatcher.HasPerfectMatch(file, xmlTitles))
                {
                    Console.WriteLine("Perfect match for " + file);
                    totalFilesProcessed++;
                    continue;
                }
                var result = fileMatcher.GetUserDefinedMatch(file, xmlTitles);
                totalFilesProcessed++;
                if (result == null)
                {
                    Console.WriteLine("No match was found for " + file + ". Press 'q' to quit or any other key to continue.");
                    var key = Console.ReadKey();
                    if (key.KeyChar == 'q')
                    {
                        return;
                    }
                }
                successfulRenames++;
            }

            // Ask for a key press to close the app
            Console.WriteLine("Processing complete!");
            Console.WriteLine("Processed " + totalFilesProcessed + " of " + totalFilesToProcess + ". " + successfulRenames + "files renamed.");
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey();
        }
    }
}

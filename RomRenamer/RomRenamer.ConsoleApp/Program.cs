using System;
using System.Collections.Generic;

namespace RomRenamer.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            // First, ask the user for the location of ROM files
            var fileRetriever = new FileRetriever(new UserReadWrite());
            var fileDirectory = fileRetriever.GetFileLocation();
            
            // Now, get the XML to compare to
            var xmlParser = new XmlParser(new UserReadWrite());
            IList<string> titleList = xmlParser.FindTitles();
            if (titleList == null)
            {
                return;
            }

            var titlesToProcess = titleList.Count;
            var titlesProcessed = 0;
            var successfulRenames = 0;
            var unMatchedFileCount = 0;
            var titlesWithNoMatch = new List<string>();

            foreach (var title in titleList)
            {
                var files = fileRetriever.GetFiles(fileDirectory);
                var fileMatcher = new FileMatcher(new UserReadWrite(), title, files);
                if (fileMatcher.HasPerfectMatch())
                {
                    Console.WriteLine(title + " has perfect match!");
                    titlesProcessed++;
                    successfulRenames++;
                    continue;
                }
                var fileToRename = fileMatcher.GetUserDefinedMatch();
                if (fileToRename == "exit")
                    break;
                titlesProcessed++;
                
                if (fileToRename == null)
                {
                    Console.WriteLine(Environment.NewLine + "No match was found for " + title + ".");
                    titlesWithNoMatch.Add(title);
                    unMatchedFileCount++;
                    continue;
                }
                
                var fileRenamer = new FileRenamer(new UserReadWrite());
                if (fileRenamer.Rename(fileToRename, title))
                {
                    successfulRenames++;
                }
            }

            // Ask for a key press to close the app
            Console.WriteLine(Environment.NewLine + "------------------------------------------");
            Console.WriteLine("Processing complete!");
            Console.WriteLine("Processed " + titlesProcessed + " of " + titlesToProcess + ". " + successfulRenames + " files renamed.");
            Console.WriteLine(unMatchedFileCount + " titles did not have a matching file."+ Environment.NewLine + Environment.NewLine + "Unmatched files:");
            foreach (var titleWithNoMatch in titlesWithNoMatch)
            {
                Console.WriteLine(titleWithNoMatch);
            }
            Console.WriteLine(Environment.NewLine + "Press any key to quit.");
            Console.ReadKey();
        }
    }
}

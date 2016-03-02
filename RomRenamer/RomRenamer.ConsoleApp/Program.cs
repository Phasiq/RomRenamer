using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace RomRenamer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // First, ask the user for the location of ROM files
            IReadOnlyCollection<string> fileList = null;
            do
            {
                Console.WriteLine("Please enter the path to the directory containing your ROM files or enter 'q' to quit.");
                var userEntry = Console.ReadLine();
                if (userEntry != null && userEntry.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }

                fileList = DirectoryFinder.Find(Console.ReadLine());
            } while (fileList == null);

            if (!fileList.Any())
            {
                Console.WriteLine("No games were found. Press any key to quit.");
                Console.ReadKey();
                return;
            }
                

            // Now, get the XML to compare to
            IReadOnlyCollection<string> doc = null;
            do
            {
                Console.WriteLine(
                    "Please enter the path and filename of the XML file containg the list of ROMs or enter 'q' to quit.");
                var userEntry = Console.ReadLine();
                if (userEntry != null && userEntry.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    return;
                }
                doc = XmlParser.GetRomTitles(userEntry);
            } while (doc == null);

            var timer = new Stopwatch();
            foreach (var file in fileList)
            {
                timer.Start();
                var result = FileMatcher.GetMatches(file, doc);

            }

            // Ask for a key press to close the app
            Console.ReadKey();
        }
    }
}

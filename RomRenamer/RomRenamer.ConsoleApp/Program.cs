using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomRenamer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // First, ask the user for the location of ROM files
            IEnumerable<string> fileList = null;
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

            // Ask for a key press to close the app
            Console.ReadKey();
        }
    }
}

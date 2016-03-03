using System;
using System.Collections;
using System.Collections.Generic;

namespace RomRenamer.ConsoleApp
{
    public static class FileSelector
    {
        public static int Select(IList<string> fileOptions)
        {
            int? selection = null;
            do
            {
                Console.WriteLine(fileOptions.Count +
                                  " matches found. Press the corresponding number to match or press '0' if no match is listed.");
                for (int i = 1; i <= fileOptions.Count; i++)
                {
                    Console.WriteLine(i + ". " + fileOptions[i-1]);
                }
                var response = Console.ReadLine();
                int selectedInteger;
                if (!int.TryParse(response, out selectedInteger))
                {
                    continue;
                }
                if (selectedInteger == 0)
                {
                    selection = selectedInteger;
                    continue;
                }
                if (selectedInteger < 0 || selectedInteger > fileOptions.Count + 1)
                {
                    Console.WriteLine("No file matches the corresponding number. Please make another selection.");
                    continue;
                }

            } while (selection == null);
        }
    }
}
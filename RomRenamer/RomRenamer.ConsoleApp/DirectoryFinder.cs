using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomRenamer.ConsoleApp
{
    public static class DirectoryFinder
    {
        public static IReadOnlyCollection<string> Find(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                Console.WriteLine("The directory path is required.");
                return null;
            }
            try
            {
                var files = Directory.GetFiles(directoryPath);
                return !files.Any() ? null : files;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("The specified directory path (" + directoryPath + " is invalid or does not exist.");
            }
            catch (PathTooLongException)
            {
                Console.WriteLine("The specified directory path is too long.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("You do not have permission to access the specified directory.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred. " + ex);
            }
            return null;
        } 
    }
}

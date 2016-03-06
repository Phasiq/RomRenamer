using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RomRenamer.ConsoleApp
{
    public class DirectoryFinder
    {
        private readonly IUserReadWrite _userInteraction;

        public DirectoryFinder(IUserReadWrite userInteraction)
        {
            _userInteraction = userInteraction;
        }

        public IReadOnlyCollection<string> Find()
        {
            do
            {
                _userInteraction.WriteLine("Please enter the path to the directory containing your ROM files or enter 'q' to quit.");
                var userEntry = _userInteraction.ReadLine();
                if (userEntry != null && userEntry.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                var files = GetFilesFromPath(userEntry);
                if (files != null && files.Any())
                {
                    return files;
                }
                _userInteraction.WriteLine("No files were found at the specified location.");
                var result = ConfirmNewPath();
                if (!result)
                {
                    return null;
                }
                
            } while (true);
        }

        private bool ConfirmNewPath()
        {
            do
            {
                _userInteraction.WriteLine("Would you like to choose a different path? y/n");
                var userEntry = _userInteraction.ReadKey();
                switch (userEntry.KeyChar)
                {
                    case 'y':
                    case 'Y':
                        return true;
                    case 'n':
                    case 'N':
                        return false;
                    default:
                    {
                        _userInteraction.WriteLine("Invalid selection. Would you like to choose a different path? y/n");
                        continue;
                    }
                }
            } while (true);
        }

        private IReadOnlyCollection<string> GetFilesFromPath(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                _userInteraction.WriteLine("The directory path is required.");
                return null;
            }
            try
            {
                var files = Directory.GetFiles(directoryPath);
                return !files.Any() ? null : files;
            }
            catch (DirectoryNotFoundException)
            {
                _userInteraction.WriteLine("The specified directory path (" + directoryPath + " is invalid or does not exist.");
            }
            catch (PathTooLongException)
            {
                _userInteraction.WriteLine("The specified directory path is too long.");
            }
            catch (UnauthorizedAccessException)
            {
                _userInteraction.WriteLine("You do not have permission to access the specified directory.");
            }
            catch (Exception ex)
            {
                _userInteraction.WriteLine("An unexpected error occurred. " + ex);
            }
            return null;
        } 
    }
}

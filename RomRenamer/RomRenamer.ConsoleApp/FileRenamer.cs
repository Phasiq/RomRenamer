using System;
using System.IO;

namespace RomRenamer.ConsoleApp
{
    public class FileRenamer
    {
        private readonly IUserReadWrite _userInteraction;

        public FileRenamer(IUserReadWrite userInteraction)
        {
            _userInteraction = userInteraction;
        }

        public bool Rename(string oldFilePathAndName, string newFilePathAndName)
        {
            try
            {
                File.Move(oldFilePathAndName,
                    Path.GetDirectoryName(oldFilePathAndName) + @"/" + newFilePathAndName + Path.GetExtension(oldFilePathAndName));
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                _userInteraction.WriteLine("The specified path for file " + oldFilePathAndName + " could not be found.");
                return false;
            }
            catch (PathTooLongException)
            {
                _userInteraction.WriteLine("The specified file path is too long. " + oldFilePathAndName);
            }
            catch (UnauthorizedAccessException)
            {
                _userInteraction.WriteLine("You do not have permission to change the file name.");
            }
            return false;
        }
    }
}
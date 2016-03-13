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

        public bool Rename(string oldFilePathAndName, string newFileName)
        {
            try
            {
                File.Move(oldFilePathAndName,
                    Path.GetDirectoryName(oldFilePathAndName) + @"/" + newFileName +
                    Path.GetExtension(oldFilePathAndName));
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
            catch (IOException)
            {
                _userInteraction.WriteLine("The destination file already exists. Would you like to replace it? y/n");
                do
                {
                    var userResponse = _userInteraction.ReadKey().KeyChar.ToString();
                    if (userResponse.Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        return ReplaceFile(oldFilePathAndName, newFileName);
                    }
                    if (userResponse.Equals("n", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                } while (true);
            }
            return false;
        }

        public bool ReplaceFile(string oldFilePathAndName, string newFileName)
        {
            File.Delete(Path.GetDirectoryName(oldFilePathAndName) + @"/" + newFileName +
                    Path.GetExtension(oldFilePathAndName));
            return Rename(oldFilePathAndName, newFileName);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace RomRenamer.ConsoleApp
{
    public class XmlParser
    {
        private readonly IUserReadWrite _userInteraction;

        public XmlParser(IUserReadWrite userInteraction)
        {
            _userInteraction = userInteraction;
        }

        public IList<string> FindTitles()
        {
            do
            {
                _userInteraction.WriteLine(
                    "Please enter the path and filename of the XML file containg the list of ROMs or enter 'q' to quit.");
                var userEntry = Console.ReadLine();
                if (userEntry != null && userEntry.Equals("q", StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                var doc = GetRomTitles(userEntry);
                if (doc != null)
                    return doc;
            } while (true);
        } 

        private IList<string> GetRomTitles(string xmlFilePath)
        {
            try
            {
                var xmlFile = XDocument.Load(xmlFilePath);
                var xElement = xmlFile.Element("menu");
                if (xElement == null)
                {
                    _userInteraction.WriteLine("No titles were found in the specified XML file.");
                    return null;
                }

                var fileList = new List<string>();

                foreach (var element in xElement.Elements())
                {
                    var attribute = element.Attribute("name");
                    if (!string.IsNullOrWhiteSpace(attribute?.Value))
                    {
                        fileList.Add(attribute.Value);
                    }
                }
                if (!fileList.Any())
                {
                    _userInteraction.WriteLine("No titles were found in the specified XML file.");
                    return null;
                }

                return fileList;
            }
            catch (PathTooLongException)
            {
                _userInteraction.WriteLine("The specified file path is too long.");
                return null;
            }
            catch (FileNotFoundException)
            {
                _userInteraction.WriteLine("The specified file '" + xmlFilePath + "' was not found.");
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                _userInteraction.WriteLine("The specified file path '" + xmlFilePath + "' is invalid.");
                return null;
            }
            catch (XmlException)
            {
                _userInteraction.WriteLine("The specified file does not contain valid XML.");
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                _userInteraction.WriteLine("You do not have permission to access the specified file.");
                return null;
            }
        }
    }
}

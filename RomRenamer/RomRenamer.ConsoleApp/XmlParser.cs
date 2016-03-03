using System;
using System.Collections.Generic;
using System.IO;
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

        public IReadOnlyCollection<string> GetRomTitles(string xmlFilePath)
        {
            XDocument file = null;
            try
            {
                file = XDocument.Load(xmlFilePath);
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
            var xElement = file.Element("menu");
            if (xElement == null)
            {
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

            return fileList;
        }
    }
}

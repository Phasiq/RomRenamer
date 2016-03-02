using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RomRenamer.ConsoleApp;

namespace RomRenamer.Tests
{
    // Format of tests should be <method>_Should<expected>_When<condition>
    [TestClass]
    public class XmlParserTests
    {
        [TestMethod]
        public void GetRomTitles_ReturnsFilenameList_ValidPathToXmlFileProvided()
        {
            var files =
                XmlParser.GetRomTitles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                               "/TestFiles/Super Nintendo Entertainment System.xml");

            Assert.IsNotNull(files);
            Assert.IsInstanceOfType(files, typeof(IEnumerable<string>));
        }

        [TestMethod]
        public void GetRomTitles_ReturnsNull_InvalidPathProvided()
        {
            var file = XmlParser.GetRomTitles(@"Y:\InvalidPathThatDoesntExists");
            Assert.IsNull(file);
        }

        [TestMethod]
        public void GetRomTitles_ReturnsNull_InvalidXmlFileProvided()
        {
            var file =
                XmlParser.GetRomTitles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                               "/TestFiles/TestFile1.txt");
            Assert.IsNull(file);
        }
    }
}
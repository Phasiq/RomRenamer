using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RomRenamer.ConsoleApp;

namespace RomRenamer.Tests
{
    [TestClass]
    public class DirectoryFinderTests
    {
        [TestMethod]
        public void Find_ReturnsTwoFiles_PassedValidPath()
        {
            var files =
                DirectoryFinder.Find(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/TestFiles");
            Assert.AreEqual(2, files.Count());
        }

        [TestMethod]
        public void Find_ReturnsNull_PassedInvalidPath()
        {
            var files = DirectoryFinder.Find(@"E:\derpytesttest");
            Assert.IsNull(files);
        }
    }
}

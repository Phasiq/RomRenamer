using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RomRenamer.ConsoleApp;
using RomRenamer.Tests.Stubs;

namespace RomRenamer.Tests
{
    [TestClass]
    public class DirectoryFinderTests
    {
        [TestMethod]
        public void Find_ReturnsTwoFiles_PassedValidPath()
        {
            var readKeys = new List<char> {};
            var readLines = new List<string> {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestRoms"};

            var testUserInteraction = new TestUserReadWrite(readKeys, readLines);

            var directoryFinder = new FileRetriever(testUserInteraction);
            var files = directoryFinder.GetFileLocation();
            Assert.AreEqual(2, files.Count);
        }

        [TestMethod]
        public void Find_ReturnsNull_PassedInvalidPath()
        {
            var readKeys = new List<char> { 'n' };
            var readLines = new List<string> { @"E:\derpytestest" };

            var testUserInteraction = new TestUserReadWrite(readKeys, readLines);

            var directoryFinder = new FileRetriever(testUserInteraction);
            var files = directoryFinder.GetFileLocation();
            Assert.IsNull(files);
        }

        [TestMethod]
        public void Find_ReturnsTwoFiles_FirstPathInvalidSecondValid()
        {
            var readKeys = new List<char> { 'y', 'n' };
            var readLines = new List<string> { @"E:\derpytestest", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestRoms" };

            var testUserInteraction = new TestUserReadWrite(readKeys, readLines);

            var directoryFinder = new FileRetriever(testUserInteraction);
            var files = directoryFinder.GetFileLocation();
            Assert.AreEqual(2, files.Count);
        }

        [TestMethod]
        public void Find_ReturnsNull_PathSearchConfirmationInvalidCharacter()
        {
            var readKeys = new List<char> { 'p','n' };
            var readLines = new List<string> { @"E:\derpytestest" };

            var testUserInteraction = new TestUserReadWrite(readKeys, readLines);

            var directoryFinder = new FileRetriever(testUserInteraction);
            var files = directoryFinder.GetFileLocation();
            Assert.IsNull(files);
            Assert.IsTrue(testUserInteraction.HasWriteLine("Invalid selection. Would you like to choose a different path? y/n"));
        }

        [TestMethod]
        public void Find_ReturnsNull_QIsEnteredWithNoFileSelected()
        {
            var readKeys = new List<char>() {};
            var readLines = new List<string>() {"q"};
            var testUserInteraction = new TestUserReadWrite(readKeys, readLines);
            var directoryFinder = new FileRetriever(testUserInteraction);
            var result = directoryFinder.GetFileLocation();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Find_ReturnsNull_EmptyFilePathEntered()
        {
            var readKeys = new List<char>() {'n' };
            var readLines = new List<string>() { " " };
            var testUserInteraction = new TestUserReadWrite(readKeys, readLines);
            var directoryFinder = new FileRetriever(testUserInteraction);
            directoryFinder.GetFileLocation();
            Assert.IsTrue(testUserInteraction.HasWriteLine("The directory path is required."));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RomRenamer.ConsoleApp;
using RomRenamer.Tests.Stubs;

namespace RomRenamer.Tests
{
    [TestClass]
    public class FileMatcherTests
    {
        [TestMethod]
        public void GetUserDefinedMatch_ReturnMatch_MatchingFilenameProvided()
        {
            var readKeys = new List<char> { '1' };
            var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };

            var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            var namesAvailable = new List<string> { "TestFile1", "Batman" };
            var fileName = "TestFile";
            var fileMatcher = new FileMatcher(testUserReadWrite, fileName, namesAvailable);
            var result = fileMatcher.GetUserDefinedMatch();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserDefinedMatch_WritesError_InvalidFileSelectionIsMade()
        {
            var readKeys = new List<char> { '7','1' };
            var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };

            var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            var namesAvailable = new List<string> { "TestFile1", "Batman" };
            var fileName = "TestFile";
            var fileMatcher = new FileMatcher(testUserReadWrite, fileName, namesAvailable);
            var result = fileMatcher.GetUserDefinedMatch();
            Assert.IsTrue(testUserReadWrite.HasWriteLine("Invalid selection. Please try again."));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserDefinedMatch_ReturnMatchWithInvalidSelectionWarning_MatchingFilenameProvided()
        {
            var readKeys = new List<char> { '2','1' };
            var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };

            var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            var namesAvailable = new List<string> { "TestFile1", "Batman" };
            var fileName = "TestFile";
            var fileMatcher = new FileMatcher(testUserReadWrite, fileName, namesAvailable);
            var result = fileMatcher.GetUserDefinedMatch();

            Assert.IsNotNull(result);
            Assert.IsTrue(testUserReadWrite.HasWriteLine("Invalid selection. Please try again."));
        }
        
        [TestMethod]
        public void GetUserDefinedMatch_ReturnsNull_MatchingFileNotFound()
        {
            var readKeys = new List<char> { };
            var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };

            var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            var namesAvailable = new List<string> { "TestFile1", "Batman" };
            var fileName = "Fs";
            var fileMatcher = new FileMatcher(testUserReadWrite, fileName, namesAvailable);
            var result = fileMatcher.GetUserDefinedMatch();

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetUserDefinedMatch_ThrowsException_PageSizeGreaterThan9()
        {
            var readKeys = new List<char> { };
            var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };

            var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            var namesAvailable = new List<string> { "TestFile1", "Batman" };
            var fileName = "Fs";
            var fileMatcher = new FileMatcher(testUserReadWrite, fileName, namesAvailable);
            fileMatcher.GetUserDefinedMatch();
        }

        [TestMethod]
        public void HasPerfectMatch_ReturnsTrue_PerfectMatchProvided()
        {
            var readKeys = new List<char> { '1' };
            var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };

            var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            var namesAvailable = new List<string> { "TestFile1", "Batman" };
            var fileName = "TestFile1";
            var fileMatcher = new FileMatcher(testUserReadWrite, fileName, namesAvailable);
            var result = fileMatcher.HasPerfectMatch();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetUserDefinedMatch_ReturnsMatch_MatchedFileOn2ndPage()
        {
            var readKeys = new List<char> { '0','1' };
            var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };

            var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            var namesAvailable = new List<string>
            {
                "TestFile1", "TestFile2", "TestFile3", "TestFile4", "TestFile5", "TestFile6",
                "TestFile7", "TestFile8", "TestFile9","TestFile10","TestFile11"
            };
            var fileName = "Te";
            var fileMatcher = new FileMatcher(testUserReadWrite, fileName, namesAvailable);
            var result = fileMatcher.GetUserDefinedMatch();

            Assert.IsNotNull(result);
            Assert.AreEqual("TestFile10", result);
        }

        [TestMethod]
        public void GetUserDefinedMatch_ReturnsNull_NoMatchesFound()
        {
            var readKeys = new List<char> { '0', '0' };
            var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };

            var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            var namesAvailable = new List<string>
            {
                "TestFile1", "TestFile2", "TestFile3", "TestFile4", "TestFile5", "TestFile6",
                "TestFile7", "TestFile8", "TestFile9","TestFile10","TestFile11"
            };
            var fileName = "Te";
            var fileMatcher = new FileMatcher(testUserReadWrite, fileName, namesAvailable);
            var result = fileMatcher.GetUserDefinedMatch();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Derp()
        {
            var fileName = "Derp Berp";
            var titleOptions = new List<string> {"Derp Berp", "Berp, Derp", "Barfy, Derp",};
            var fileMatcher =
                new FileMatcher(new TestUserReadWrite(new List<char> {'h'}, new List<string> {"Test", "Derp"}),
                    fileName, titleOptions);
            var result = fileMatcher.LevenshteinDistance(fileName, titleOptions[0]);
            var result2 = fileMatcher.LevenshteinDistance(fileName, titleOptions[1]);
            var result3 = fileMatcher.LevenshteinDistance(fileName, titleOptions[2]);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.IsNotNull(result3);
        }
    }
}
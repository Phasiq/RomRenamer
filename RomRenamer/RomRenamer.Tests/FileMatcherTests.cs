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
        public void GetMatches_ReturnMatch_MatchingFilenameProvided()
        {
            throw new NotImplementedException();
            //var readKeys = new List<char> { };
            //var readLines = new List<string> { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestFiles" };
            
            //var testUserReadWrite = new TestUserReadWrite(readKeys, readLines);
            //var namesAvailable = new List<string> {"Superman", "Batman"};
            //var fileName = "bat";
            //var fileMatcher = new FileMatcher(testUserReadWrite);
            //var result = fileMatcher.GetUserDefinedMatch(fileName, namesAvailable);
            //Assert.IsNotNull(result);
            //Assert.AreEqual(1, result.Count);
        }
    }
}
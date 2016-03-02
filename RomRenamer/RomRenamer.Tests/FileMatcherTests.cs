using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RomRenamer.ConsoleApp;

namespace RomRenamer.Tests
{
    [TestClass]
    public class FileMatcherTests
    {
        [TestMethod]
        public void GetMatches_ReturnMatch_MatchingFilenameProvided()
        {
            var namesAvailable = new List<string> {"Superman", "Batman"};
            var fileName = "bat";
            var result = FileMatcher.GetMatches(fileName, namesAvailable);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }
    }
}
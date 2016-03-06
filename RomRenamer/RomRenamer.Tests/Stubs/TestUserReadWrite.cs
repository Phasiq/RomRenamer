using System;
using System.Collections.Generic;
using System.Linq;
using RomRenamer.ConsoleApp;

namespace RomRenamer.Tests.Stubs
{
    public class TestUserReadWrite : IUserReadWrite
    {
        private readonly List<char> _readKeys;
        private readonly List<string> _readLines;
        private List<string> _write;
        private List<string> _writeLine;  
        private int _readKeyIndex;
        private int _readLinesIndex;

        public TestUserReadWrite(List<char> readKeys, List<string> readLines)
        {
            _readKeyIndex = 0;
            _readLinesIndex = 0;
            _readLines = readLines;
            _readKeys = readKeys;
            _write = new List<string>();
            _writeLine = new List<string>();
        }

        public ConsoleKeyInfo ReadKey()
        {
            if (_readKeyIndex >= _readKeys.Count)
                throw new ArgumentException("ReadKey Index in test class is invalid.");
            var consoleKey = new ConsoleKeyInfo(_readKeys[_readKeyIndex++], ConsoleKey.NoName, false, false, false);
            return consoleKey;
        }

        public string ReadLine()
        {
            if (_readLinesIndex >= _readLines.Count)
                throw new ArgumentException("ReadLine Index in test class is invalid.");
            return _readLines[_readLinesIndex++];
        }

        public void Write(string value)
        {
            _write.Add(value);
        }

        public void WriteLine(string value)
        {
            _writeLine.Add(value);
        }

        public bool HasWriteLine(string value)
        {
            return _writeLine.Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        public bool HasWrite(string value)
        {
            return _write.Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
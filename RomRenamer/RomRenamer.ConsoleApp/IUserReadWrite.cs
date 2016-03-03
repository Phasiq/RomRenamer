using System;

namespace RomRenamer.ConsoleApp
{
    public interface IUserReadWrite
    {
        ConsoleKeyInfo ReadKey();
        string ReadLine();
        void Write(string value);
        void WriteLine(string value);
    }
}
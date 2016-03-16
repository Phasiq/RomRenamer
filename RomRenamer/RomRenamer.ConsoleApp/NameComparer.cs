namespace RomRenamer.ConsoleApp
{
    public class NameComparer
    {
        public NameComparer(int score, string sourceFileName, string targetFileName)
        {
            Score = score;
            SourceFileName = sourceFileName;
            TargetFileName = targetFileName;
        }

        public int Score { get; private set; } 
        public string SourceFileName { get; private set; }
        public string TargetFileName { get; private set; }
    }
}
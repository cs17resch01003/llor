namespace LLOR.Repair
{
    using CommandLine;

    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "The program to repair.")]
        public string FilePath { get; set; } = string.Empty;
        
        [Option('t', "testonly", Required = false, HelpText = "Run the repairer without generating files.")]
        public bool TestOnly { get; set; } = false;
    }
}

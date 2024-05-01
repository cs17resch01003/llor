namespace LLOR.TestRunner
{
    using CommandLine;

    public class Options
    {
        [Option('f', "folder", Required = true, HelpText = "The folder containing the tests.")]
        public string FolderPath { get; set; } = string.Empty;

        [Option('t', "timeout", Required = false, HelpText = "Timeout for a single file benchmark.")]
        public int SingleFileTimeout { get; set; } = 5*60*1000;

        [Option('m', "mftimeout", Required = false, HelpText = "Timeout for a multi-file benchmark.")]
        public int MultiFileTimeout { get; set; } = 60*1000;
    }
}

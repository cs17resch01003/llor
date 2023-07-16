namespace LLOR.Repair
{
    using CommandLine;

    public class Options
    {
        [Option('v', "llov", Required = false, HelpText = "Path to LLOV binaries.")]
        public string BinariesPath { get; set; } = "/datadrive/llov/build";

        [Option('i', "include", Required = false, HelpText = "Include path for gcc.")]
        public string IncludePath { get; set; } = "/usr/lib/gcc/x86_64-linux-gnu/11/include/";

        [Option('t', "testonly", Required = false, HelpText = "Run the repairer without generating files.")]
        public bool TestOnly { get; set; } = false;

        [Option('s', "summaryonly", Required = false, HelpText = "Keep only the summary file.")]
        public bool SummaryOnly { get; set; } = false;

        [Option('f', "file", Required = true, HelpText = "The program to repair.")]
        public string FilePath { get; set; } = string.Empty;
    }
}

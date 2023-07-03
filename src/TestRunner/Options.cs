namespace LLOR.TestRunner
{
    using CommandLine;

    public class Options
    {
        [Option('r', "llor", Required = false, HelpText = "Path to LLOR binaries.")]
        public string LLORBinariesPath { get; set; } = "/devel/llor/src/Repair/bin/Debug/net6.0/Repair";

        [Option('v', "llov", Required = false, HelpText = "Path to LLOV binaries.")]
        public string LLOVBinariesPath { get; set; } = "/devel/llov/build/";

        [Option('i', "include", Required = false, HelpText = "Include path for LLOV.")]
        public string IncludePath { get; set; } = "/usr/lib/gcc/x86_64-linux-gnu/11/include/";

        [Option('t', "threads", Required = false, HelpText = "Number of threads.")]
        public int Threads { get; set; } = 1;

        [Option('o', "verify", Required = false, HelpText = "Run the verifier.")]
        public bool Verify { get; set; } = false;

        [Option('s', "summaryonly", Required = false, HelpText = "Keep only the summary file.")]
        public bool SummaryOnly { get; set; } = false;

        [Option('f', "folder", Required = true, HelpText = "The folder containing the tests.")]
        public string FolderPath { get; set; } = string.Empty;
    }
}
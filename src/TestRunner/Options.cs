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

        [Option('t', "verifyonly", Required = false, HelpText = "Run only the verifier.")]
        public bool VerifyOnly { get; set; } = false;

        [Option('f', "folder", Required = true, HelpText = "The folder containing the tests.")]
        public string FolderPath { get; set; } = string.Empty;
    }
}

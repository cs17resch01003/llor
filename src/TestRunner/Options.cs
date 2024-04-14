namespace LLOR.TestRunner
{
    using CommandLine;

    public class Options
    {
        [Option('f', "folder", Required = true, HelpText = "The folder containing the tests.")]
        public string FolderPath { get; set; } = string.Empty;

        [Option('t', "type", Required = false, HelpText = "Type of the testcases.")]
        public string Type { get; set; } = "file";
        
        [Option('v', "verify", Required = false, HelpText = "Run only the verifier.")]
        public bool Verify { get; set; } = false;

        [Option('r', "repair", Required = false, HelpText = "Run only the repairer.")]
        public bool Repair { get; set; } = false;

        [Option('c', "check", Required = false, HelpText = "Run the complete check")]
        public bool Check { get; set; } = false;
    }
}

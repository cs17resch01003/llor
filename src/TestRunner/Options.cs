namespace LLOR.TestRunner
{
    using CommandLine;

    public class Options
    {
        [Option('f', "folder", Required = true, HelpText = "The folder containing the tests.")]
        public string FolderPath { get; set; } = string.Empty;
        
        [Option('o', "verify", Required = false, HelpText = "Run only the verifier.")]
        public bool Verify { get; set; } = false;

        [Option('o', "repair", Required = false, HelpText = "Run only the repairer.")]
        public bool Repair { get; set; } = false;
    }
}

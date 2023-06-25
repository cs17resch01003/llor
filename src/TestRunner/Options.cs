namespace TestRunner
{
    using CommandLine;

    public class Options
    {
        [Option('f', "folder", Required = true, HelpText = "The folder containing the tests.")]
        public string FolderPath { get; set; } = string.Empty;
    }
}

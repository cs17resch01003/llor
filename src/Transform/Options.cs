namespace LLOR.Transform
{
    using CommandLine;

    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "The program to transform.")]
        public string FilePath { get; set; } = string.Empty;
    }
}

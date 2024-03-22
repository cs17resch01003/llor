namespace LLOR.TestRunner
{
    using CsvHelper.Configuration.Attributes;
    
    public class SourceCode
    {
        [Name("filepath")]
        public string? FilePath { get; set; }

        [Name("lines")]
        public int Lines { get; set; }

        [Name("instructions")]
        public int Instructions { get; set; }

        [Name("barriers")]
        public int Barriers { get; set; }
    }
}

namespace LLOR.TestRunner
{
    using System.IO;
    using CsvHelper.Configuration.Attributes;
    
    public class Summary
    {
        [Ignore]
        public FileSystemInfo? Source { get; set; }

        [Name("path")]
        public string? Path { get; set; }

        [Name("lines")]
        public double? Lines { get; set; }

        [Name("insts")]
        public double? Instructions { get; set; }

        [Name("barrs")]
        public double? Barriers { get; set; }

        [Name("vresult")]
        public string? VerificationResult { get; set; }

        [Name("mhs_res")]
        public string? MhsResult { get; set; }

        [Name("mhs_time")]
        public double MhsTimeTaken { get; set; }

        [Name("mhs_c")]
        public double MhsCount { get; set; }

        [Name("mhs_sc")]
        public double MhsSolverCount { get; set; }

        [Name("max_res")]
        public string? MaxResult { get; set; }

        [Name("max_time")]
        public double MaxTimeTaken { get; set; }

        [Name("max_sc")]
        public double MaxSolverCount { get; set; }
    }
}

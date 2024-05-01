namespace LLOR.TestRunner
{
    using CsvHelper.Configuration.Attributes;
    
    public class Summary
    {
        [Name("path")]
        public string? Path { get; set; }

        [Name("lines")]
        public int Lines { get; set; }

        [Name("insts")]
        public int Instructions { get; set; }

        [Name("barrs")]
        public int Barriers { get; set; }

        [Name("vresult")]
        public string? VerificationResult { get; set; }

        [Name("mhs_res")]
        public string? MhsResult { get; set; }

        [Name("mhs_time")]
        public long MhsTimeTaken { get; set; }

        [Name("mhs_c")]
        public int MhsCount { get; set; }

        [Name("mhs_sc")]
        public int MhsSolverCount { get; set; }

        [Name("max_res")]
        public string? MaxResult { get; set; }

        [Name("max_time")]
        public long MaxTimeTaken { get; set; }

        [Name("max_sc")]
        public int MaxSolverCount { get; set; }
    }
}

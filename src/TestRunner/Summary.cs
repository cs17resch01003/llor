namespace LLOR.TestRunner
{
    using CsvHelper.Configuration.Attributes;
    
    public class Summary
    {
        [Name("path")]
        public string? Path { get; set; }

        [Name("vresult")]
        public string? VerificationResult { get; set; }

        [Name("mhs_result")]
        public string? MhsResult { get; set; }

        [Name("mhs_change")]
        public int MhsChanges { get; set; }

        [Name("mhs_time")]
        public long MhsTimeTaken { get; set; }

        [Name("max_result")]
        public string? MaxResult { get; set; }

        [Name("max_change")]
        public int MaxChanges { get; set; }

        [Name("max_time")]
        public long MaxTimeTaken { get; set; }
    }
}

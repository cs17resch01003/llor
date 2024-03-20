namespace LLOR.TestRunner
{
    using CsvHelper.Configuration.Attributes;
    
    public class Record
    {
        [Name("filepath")]
        public string? FilePath { get; set; }

        [Name("lines")]
        public int Lines { get; set; }

        [Name("instructions")]
        public int Instructions { get; set; }

        [Name("barriers")]
        public int Barriers { get; set; }

        [Name("vresult")]
        public string? VerificationResult { get; set; }

        [Name("mhs_result")]
        public string? MhsResult { get; set; }

        [Name("mhs_time")]
        public long MhsTimeTaken { get; set; }

        [Name("max_result")]
        public string? MaxResult { get; set; }

        [Name("max_time")]
        public long MaxTimeTaken { get; set; }
    }
}

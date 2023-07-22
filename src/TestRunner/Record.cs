namespace LLOR.TestRunner
{
    using CsvHelper.Configuration.Attributes;
    
    public class Record
    {
        [Name("filepath")]
        public string? FilePath { get; set; }

        [Name("vresult")]
        public string? VerificationResult { get; set; }

        [Name("rresult")]
        public string? RepairResult { get; set; }

        [Name("fresult")]
        public string? RepairedResult { get; set; }

        [Name("changes")]
        public int? Changes { get; set; }

        [Name("timetaken")]
        public long? TimeTaken { get; set; }
    }
}

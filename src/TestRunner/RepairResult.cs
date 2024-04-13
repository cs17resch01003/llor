namespace LLOR.TestRunner
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CsvHelper.Configuration.Attributes;
    using LLOR.Common;

    public class RepairResult
    {
        [Name("path")]
        public string? Path { get; set; }

        [Name("statuscode")]
        public string? StatusCode { get; set; }

        [Name("time")]
        public long TimeTaken { get; set; }

        [Ignore]
        public int Lines { get; set; }

        [Ignore]
        public int Instructions { get; set; }

        [Ignore]
        public int Barriers { get; set; }

        [Name("vcount")]
        public int VerificationCount { get; set; }

        [Name("vtime")]
        public long VerificationTime { get; set; }

        [Name("mhscount")]
        public int MhsCount { get; set; }

        [Name("mhstime")]
        public long MhsTime { get; set; }

        [Name("scount")]
        public int SolverCount { get; set; }

        [Name("stime")]
        public long SolverTime { get; set; }

        [Name("ttime")]
        public long TotalTime { get; set; }

        [Name("changes")]
        public int Changes { get; set; }

        public Dictionary<StatusCode, int> Statuses { get; set; } = new Dictionary<StatusCode, int>();

        [Ignore]
        public bool Assert { get; set; }
    }
}

namespace LLOR.TestRunner
{
    using System.Collections.Generic;
    using LLOR.Common;

    public class RepairResult
    {
        public string? Path { get; set; }

        public string? StatusCode { get; set; }

        public int Lines { get; set; }

        public int Instructions { get; set; }

        public int Barriers { get; set; }

        public int VerificationCount { get; set; }

        public long VerificationTime { get; set; }

        public int MhsCount { get; set; }

        public long MhsTime { get; set; }

        public int SolverCount { get; set; }

        public long SolverTime { get; set; }

        public long TotalTime { get; set; }

        public int Changes { get; set; }

        public Dictionary<StatusCode, int> Statuses { get; set; } = new Dictionary<StatusCode, int>();

        public bool Assert { get; set; }
    }
}

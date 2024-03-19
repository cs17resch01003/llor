namespace LLOR.TestRunner
{
    using LLOR.Common;

    public class RepairResult
    {
        public StatusCode StatusCode { get; set; }

        public long TimeTaken { get; set; }

        public int Lines { get; set; }

        public int Instructions { get; set; }

        public int Barriers { get; set; }

        public int SolverCount { get; set; }

        public int Changes { get; set; }

        public bool Assert { get; set; }
    }
}

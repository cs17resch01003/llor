namespace LLOR.TestRunner
{
    using LLOR.Common;

    public class RepairResult
    {
        public StatusCode StatusCode { get; set; }

        public int Changes { get; set; }

        public bool Assert { get; set; }
    }
}

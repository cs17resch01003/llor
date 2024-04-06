namespace LLOR.Repair
{
    using CommandLine;

    public class Options
    {
        private bool testOnly = false;

        [Option('p', "path", Required = true, HelpText = "The program to repair.")]
        public string Path { get; set; } = string.Empty;
        
        [Option('d', "detailedlogging", Required = false, HelpText = "Enables detailed logging.")]
        public bool DetailedLogging { get; set; } = false;

        [Option('t', "testonly", Required = false, HelpText = "Run the repairer without generating files.")]
        public bool TestOnly
        {
            get
            {
                return testOnly;
            }

            set
            {
                DetailedLogging = testOnly = value;
            }
        }

        [Option('s', "solvertype", Required = false, HelpText = "The solver that is to be used in the repair process.")]
        public Solver.SolverType SolverType { get; set; } = Solver.SolverType.mhs;
    }
}

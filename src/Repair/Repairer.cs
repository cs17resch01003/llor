namespace LLOR.Repair
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using LLOR.Common;
    using LLOR.Repair.Exceptions;
    
    public class Repairer
    {
        private Verifier verifier;

        private Instrumentor instrumentor;

        public List<DataRace> Races { get; set; } = new List<DataRace>();

        public Repairer(Verifier verifier, Instrumentor instrumentor)
        {
            this.verifier = verifier;
            this.instrumentor = instrumentor;
        }

        public Dictionary<string, bool> Repair(Options options, CancellationToken ct)
        {
            Solver.SolverType solverType = options.SolverType;
            Dictionary<string, bool> assignments = new Dictionary<string, bool>();

            try
            {
                verifier.ValidateSource();
                while (true)
                {
                    ct.ThrowIfCancellationRequested();

                    IEnumerable<DataRace> current_races = verifier.Verify();
                    if (!current_races.Any())
                    {
                        if (solverType == Solver.SolverType.mhs)
                            solverType = Solver.SolverType.Optimizer;
                        else
                            return assignments;
                    }

                    if (current_races.Any() && current_races.All(x => x.Sink != null && x.Sink == x.Source))
                        throw new RepairException(
                            StatusCode.RepairError,
                            "Encountered a write-write race on the same line.");

                    foreach (DataRace race in current_races)
                        race.PopulateMetadata(instrumentor.Metadata.Barriers.Values);
                    Races.AddRange(current_races);

                    Solver solver = new Solver();
                    if (solverType == Solver.SolverType.Optimizer)
                    {
                        if (assignments.Count(x => x.Value == true) >= 2)
                        {
                            Dictionary<string, bool> temp = solver.Optimize(Races, assignments);
                            if (temp.Where(x => x.Value).Count() >= assignments.Where(x => x.Value).Count())
                                return assignments;
                        }
                        else
                            return assignments;
                    }
                    else
                        assignments = solver.Solve(Races, solverType);

                    instrumentor.Update(assignments);
                }
            }
            catch (RepairException)
            {
                IEnumerable<DataRace> races = verifier.VerifySource();
                if (races.Any())
                    throw;

                assignments.Clear();
            }
            catch (UnsupportedException)
            {
                IEnumerable<DataRace> races = verifier.VerifySource();
                if (races.Any())
                    throw;

                assignments.Clear();
            }

            return assignments;
        }
    }
}

namespace LLOR.Repair
{
    using System.Collections.Generic;
    using System.Linq;
    using LLOR.Common;
    using LLOR.Repair.Exceptions;
    
    public class Repairer
    {
        private Verifier verifier;

        private Instrumentor instrumentor;

        public Repairer(Verifier verifier, Instrumentor instrumentor)
        {
            this.verifier = verifier;
            this.instrumentor = instrumentor;
        }

        public Dictionary<string, bool> Repair()
        {
            Dictionary<string, bool> assignments = new Dictionary<string, bool>();
            try
            {
                List<DataRace> races = new List<DataRace>();
                while (true)
                {
                    IEnumerable<DataRace> current_races = verifier.Verify();
                    if (!current_races.Any())
                        return assignments;

                    if (current_races.All(x => x.Sink != null && x.Sink == x.Source))
                        throw new RepairException(
                            StatusCode.RepairError,
                            "Encountered a write-write race on the same line.");

                    foreach (DataRace race in current_races)
                        race.PopulateBarriers(instrumentor.Barriers.Values);
                    races.AddRange(current_races);

                    Solver solver = new Solver();
                    assignments = solver.Solve(races);

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

            return assignments;
        }
    }
}

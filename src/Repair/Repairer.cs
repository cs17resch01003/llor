namespace LLOR.Repair
{
    using System.Collections.Generic;
    using System.Linq;
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
            while (true)
            {
                IEnumerable<DataRace> races = verifier.Verify();
                if (!races.Any())
                    return assignments;

                if (races.All(x => x.Sink == x.Source))
                    throw new RepairException("Encountered a write-write race on the same line.");

                races = races.Where(x => x.Sink != null && !x.Sink.Equals(x.Source));
                foreach (DataRace race in races)
                    race.PopulateBarriers(instrumentor.Barriers.Values);

                Solver solver = new Solver();
                assignments = solver.Solve(races);

                instrumentor.Update(assignments);
            }
        }
    }
}

namespace LLOR
{
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
                List<DataRace> races = verifier.Verify();
                if (!races.Any())
                    return assignments;

                foreach (DataRace race in races)
                    race.PopulateBarriers(instrumentor.Barriers.Values);

                Solver solver = new Solver();
                assignments = solver.Solve(races);

                instrumentor.Update(assignments);
            }
        }
    }
}

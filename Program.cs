namespace LLOR
{
    public class Program
    {
        public static void Main(String[] args)
        {
            Verifier verifier = new Verifier(args[0], args[1], args[2]);
            verifier.Initialize();

            Instrumentor instrumentor = new Instrumentor(args[0], args[2]);
            Repairer repairer = new Repairer(verifier, instrumentor);

            Dictionary<string, bool> assignments = repairer.Repair();

            SummaryGenerator generator = new SummaryGenerator(args[2], instrumentor);
            IEnumerable<string> changes = generator.GenerateSummary(assignments);

            foreach (string change in changes)
                Console.WriteLine(change);
        }
    }
}

namespace LLOR
{
    public class SummaryGenerator
    {
        private FileInfo inputFile;

        private Instrumentor instrumentor;

        public SummaryGenerator(string inputFile, Instrumentor instrumentor)
        {
            this.inputFile = new FileInfo(inputFile);
            this.instrumentor = instrumentor;
        }

        public IEnumerable<string> GenerateSummary(Dictionary<string, bool> assignments)
        {
            List<string> lines = new List<string>();
            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));

            GenerateBarrierSummary(assignments, lines);
            GenerateOrderedSummary(assignments, lines);

            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            string summary_path = basePath + Path.DirectorySeparatorChar + baseName + ".summary";
            File.WriteAllLines(summary_path, lines.Distinct());

            return lines;
        }

        private void GenerateBarrierSummary(Dictionary<string, bool> assignments, List<string> lines)
        {
            IEnumerable<Barrier> barriers = assignments.Where(x => x.Value)
                .Select(x => instrumentor.Barriers[x.Key])
                .Where(x => x.BarrierType == "barrier");
            foreach(Barrier barrier in barriers)
            {
                // an existing barrier at the right place will be at line-1
                int line = barrier.Location.Line;
                bool check = !instrumentor.Existing
                    .Where(x => x.BarrierType == "barrier")
                    .Any(x => x.Location.Line == line-1);

                if (check)
                    lines.Add($"Add a barrier at line number {line}.");
            }

            IEnumerable<ExistingBarrier> existingBarriers =
                instrumentor.Existing.Where(x => x.BarrierType == "barrier");
            foreach (ExistingBarrier existing in existingBarriers)
            {
                bool keepExisting = false;
                barriers = assignments.Where(x => x.Value)
                    .Select(x => instrumentor.Barriers[x.Key])
                    .Where(x => x.BarrierType == "barrier");
                foreach(Barrier barrier in barriers)
                {
                    // an existing barrier at the right place will be at line-1
                    int line = barrier.Location.Line;
                    if (existing.Location.Line == line-1)
                    {
                        keepExisting = true;
                        break;
                    }
                }

                if (!keepExisting)
                    lines.Add($"Remove the barrier at line number {existing.Location.Line}.");
            }
        }
        
        private void GenerateOrderedSummary(Dictionary<string, bool> assignments, List<string> lines)
        {
            IEnumerable<Barrier> barriers = assignments.Where(x => x.Value)
                .Select(x => instrumentor.Barriers[x.Key])
                .Where(x => x.BarrierType == "ordered");
            foreach (string function in barriers.Select(x => x.Function).Distinct())
            {
                int min = barriers.Where(x => x.Function == function).Min(x => x.Location.Line);
                int max = barriers.Where(x => x.Function == function).Max(x => x.Location.Line);

                lines.Add("Create an ordered region covering line"
                    + (min == max ? " " : "s ")
                    + (min == max ? min : $"{min} to {max}") + ".");
            }
        }
    }
}

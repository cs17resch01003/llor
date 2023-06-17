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
            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            List<string> lines = new List<string>();
            foreach(string barrierName in assignments.Where(x => x.Value).Select(x => x.Key))
            {
                // an existing barrier at the right place will be at line-1
                int line = instrumentor.Barriers[barrierName].Location.Line;
                if (!instrumentor.Existing.Any(x => x.Line == line-1))
                    lines.Add($"Add a barrier at line number {line}.");
            }

            foreach (Location location in instrumentor.Existing)
            {
                bool keepExisting = false;
                foreach(string barrierName in assignments.Where(x => x.Value).Select(x => x.Key))
                {
                    // an existing barrier at the right place will be at line-1
                    int line = instrumentor.Barriers[barrierName].Location.Line;
                    if (location.Line == line-1)
                    {
                        keepExisting = true;
                        break;
                    }
                }

                if (!keepExisting)
                    lines.Add($"Remove the barrier at line number {location.Line}.");
            }

            string summary_path = basePath + Path.DirectorySeparatorChar + baseName + ".summary";
            File.WriteAllLines(summary_path, lines.Distinct());

            return lines;
        }
    }
}

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
                int line = instrumentor.Barriers[barrierName].Location.Line;
                lines.Add($"Add a barrier at line number {line}.");
            }

            string summary_path = basePath + Path.DirectorySeparatorChar + baseName + ".summary";
            File.WriteAllLines(summary_path, lines);

            return lines;
        }
    }
}

namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SummaryGenerator
    {
        private FileInfo inputFile;

        private Instrumentor instrumentor;

        private string language;

        private string[] fileContent;

        public SummaryGenerator(string inputFile, Instrumentor instrumentor)
        {
            this.inputFile = new FileInfo(inputFile);
            this.instrumentor = instrumentor;

            string extension = this.inputFile.Extension;
            language = extension.Equals(".f95", StringComparison.InvariantCultureIgnoreCase) ? "Fortran" : "C";
            fileContent = File.ReadAllLines(inputFile);
        }

        public IEnumerable<string> GenerateSummary(Dictionary<string, bool> assignments)
        {
            List<string> lines = new List<string>();
            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));

            GenerateBarrierSummary(assignments, lines);
            GenerateOrderedSummary(assignments, lines);
            lines = lines.OrderBy(x => x).ToList();

            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            string summary_path = basePath + Path.DirectorySeparatorChar + baseName + ".summary";
            if (lines.Any())
                File.WriteAllLines(summary_path, lines.Distinct());

            return lines;
        }

        private void GenerateBarrierSummary(Dictionary<string, bool> assignments, List<string> lines)
        {
            int add = 0, remove = 0;
            List<string> temp = new List<string>();

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
                {
                    temp.Add($"Add a barrier at line number {line}.");
                    add++;
                }
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
                {
                    string line = fileContent[existing.Location.Line-1];
                    line = Regex.Replace(line, @"\s+", " ").Trim();

                    // avoid removing implicit barriers
                    if ((language == "C" && line.Contains("pragma omp barrier")) ||
                        (language == "Fortran" && line.Contains("!$omp barrier")))
                    {
                        temp.Add($"Remove the barrier at line number {existing.Location.Line}.");
                        remove++;
                    }
                }
            }

            if (add != remove)
                lines.AddRange(temp);
        }
        
        private void GenerateOrderedSummary(Dictionary<string, bool> assignments, List<string> lines)
        {
            int add = 0, remove = 0;
            List<string> temp = new List<string>();

            IEnumerable<Barrier> barriers = assignments.Where(x => x.Value)
                .Select(x => instrumentor.Barriers[x.Key])
                .Where(x => x.BarrierType == "ordered");
            foreach (string function in barriers.Select(x => x.Function).Distinct())
            {
                IEnumerable<string?> loops = barriers.Where(x => x.Function == function)
                    .Select(x => x.Loop).Distinct();
                foreach (string? loop in loops)
                {
                    int min = barriers.Where(x => x.Function == function && x.Loop == loop)
                        .Min(x => x.Location.Line);
                    int max = barriers.Where(x => x.Function == function && x.Loop == loop)
                        .Max(x => x.Location.Line);

                    // overapproximate for Fortran programs
                    if (language == "Fortran")
                    {
                        min = instrumentor.Barriers.Values
                            .Where(x => x.Function == function && x.Loop == loop)
                            .Min(x => x.Location.Line);
                        max = instrumentor.Barriers.Values
                            .Where(x => x.Function == function && x.Loop == loop)
                            .Max(x => x.Location.Line);
                    }

                    if (min == max)
                    {
                        // an existing ordered region at the right place will be at line-1
                        bool check = !instrumentor.Existing
                            .Where(x => x.BarrierType == "ordered")
                            .Any(x => x.Location.Line == max-1);
                        
                        if (check)
                        {
                            temp.Add($"Create an ordered region covering line {max}.");
                            add++;
                        }
                    }
                    else
                    {
                        temp.Add($"Create an ordered region covering lines {min} to {max}.");
                        add++;
                    }
                }
            }

            IEnumerable<ExistingBarrier> existingBarriers =
                instrumentor.Existing.Where(x => x.BarrierType == "ordered");
            foreach (ExistingBarrier existing in existingBarriers)
            {
                bool keepExisting = false;
                barriers = assignments.Where(x => x.Value)
                    .Select(x => instrumentor.Barriers[x.Key])
                    .Where(x => x.BarrierType == "ordered");
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
                {
                    temp.Add($"Remove the ordered region at line number {existing.Location.Line}.");
                    remove++;
                }
            }

            if (add != remove)
                lines.AddRange(temp);
        }
    }
}

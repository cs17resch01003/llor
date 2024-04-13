namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SummaryGenerator
    {
        private Repairer repairer;

        private Verifier verifier;

        private Metadata metadata;

        private string? language;

        private string[]? fileContent;

        public SummaryGenerator(
            Verifier verifier, Repairer repairer, Metadata metadata)
        {
            this.repairer = repairer;
            this.verifier = verifier;
            this.metadata = metadata;

            if (metadata.SourceFile != null)
            {
                string extension = metadata.SourceFile .Extension;
                language = extension.Equals(".f95", StringComparison.InvariantCultureIgnoreCase) ? "Fortran" : "C";
                fileContent = File.ReadAllLines(metadata.SourceFile.FullName);
            }
        }

        public static void WriteSummary(string basePath, IEnumerable<string> lines)
        {
            string summary_path = basePath + Path.DirectorySeparatorChar + "summary.summary";
            if (lines.Any())
                File.WriteAllLines(summary_path, lines.Distinct());
        }

        public IEnumerable<string> GenerateSummary(Dictionary<string, bool> assignments, bool includeFilename = false)
        {
            List<string> lines = new List<string>();
            GenerateSummary(assignments, lines, includeFilename);

            return lines;
        }

        public void WriteSummary(IEnumerable<string> lines)
        {
            string basePath = metadata.BasePath + Path.DirectorySeparatorChar + metadata.BaseName;
            string summary_path = basePath + ".summary";
            if (lines.Any())
                File.WriteAllLines(summary_path, lines.Distinct());
        }

        private void GenerateSummary(Dictionary<string, bool> assignments, List<string> lines, bool includeFilename)
        {
            (bool, List<string>) barriers = GenerateBarrierSummary(assignments, includeFilename);
            (bool, List<string>) ordereds = GenerateOrderedSummary(assignments, includeFilename);

            if (barriers.Item1 || ordereds.Item1)
            {
                List<DataRace> races = verifier.VerifySource();
                if (!races.Any())
                    return;
            }

            lines.AddRange(barriers.Item2);
            lines.AddRange(ordereds.Item2);
        }

        private (bool, List<string>) GenerateBarrierSummary(Dictionary<string, bool> assignments, bool includeFilename)
        {
            (List<int>, List<int>) barriers = GenerateBarrierSummary(assignments);
            List<int> add = barriers.Item1;
            List<int> remove = barriers.Item2;

            bool barriers_check = (add.Any() || remove.Any()) && (
                add.Count == remove.Count ||
                add.Count - remove.Count >= metadata.Existing.Count(x => x.BarrierType == "barrier"));

            string? sourceFile = metadata.SourceFile?.FullName;
            string fileDescription = includeFilename && sourceFile != null ? $" in {sourceFile}" : string.Empty;

            List<string> lines = new List<string>();
            foreach (int line in add)
                lines.Add($"Add a barrier at line number {line}{fileDescription}.");

            foreach (int line in remove)
                lines.Add($"Remove the barrier at line number {line}{fileDescription}.");

            return (barriers_check, lines);
        }

        private (List<int>, List<int>) GenerateBarrierSummary(Dictionary<string, bool> assignments)
        {
            List<int> add = new List<int>();
            List<int> remove = new List<int>();

            IEnumerable<Barrier> barriers = assignments.Where(x => x.Value)
                .Select(x => metadata.Barriers[x.Key])
                .Where(x => x.BarrierType == "barrier");
            foreach(Barrier barrier in barriers)
            {
                // an existing barrier at the right place will be at line-1
                int line = barrier.Location.Line;
                bool check = !metadata.Existing.Where(x => x.BarrierType == "barrier")
                    .Any(x => x.Location.Line == line-1);

                if (check)
                    add.Add(line);
            }

            foreach (Barrier existing in metadata.Existing.Where(x => x.BarrierType == "barrier"))
            {
                bool keepExisting = false;
                barriers = assignments.Where(x => x.Value)
                    .Select(x => metadata.Barriers[x.Key])
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
                    bool check = false;
                    if (fileContent == null)
                        check = true;
                    else
                    {
                        if (fileContent.Length < existing.Location.Line)
                            continue;

                        string line = fileContent[existing.Location.Line-1];
                        line = Regex.Replace(line, @"\s+", " ").Trim();

                        // avoid removing implicit barriers
                        if ((language == "C" && line.Contains("pragma omp barrier")) ||
                            (language == "Fortran" && line.Contains("!$omp barrier")))
                        {
                            check = true;
                        }
                    }

                    if (check)
                        remove.Add(existing.Location.Line);
                }
            }

            add = add.Distinct().OrderBy(x => x).ToList();
            remove = remove.Distinct().OrderBy(x => x).ToList();

            return (add, remove);
        }

        private (bool, List<string>) GenerateOrderedSummary(Dictionary<string, bool> assignments, bool includeFilename)
        {
            (List<(int, int)>, List<int>) ordered = GenerateOrderedSummary(assignments);
            List<(int, int)> create = ordered.Item1;
            List<int> remove = ordered.Item2;

            bool ordered_check = (create.Any() || remove.Any()) && (
                create.Count == remove.Count ||
                create.Count - remove.Count >= metadata.Existing.Count(x => x.BarrierType == "ordered"));

            string? sourceFile = metadata.SourceFile?.FullName;
            string fileDescription = includeFilename && sourceFile != null ? $" in {sourceFile}" : string.Empty;

            List<string> lines = new List<string>();
            foreach ((int, int) range in create)
            {
                if (range.Item1 == range.Item2)
                    lines.Add($"Create an ordered region covering line {range.Item1}{fileDescription}.");
                else
                    lines.Add($"Create an ordered region covering lines {range.Item1} to {range.Item2}{fileDescription}.");
            }

            foreach (int line in remove)
                lines.Add($"Remove the ordered region at line number {line}{fileDescription}.");

            return (ordered_check, lines);
        }

        private (List<(int, int)>, List<int>) GenerateOrderedSummary(Dictionary<string, bool> assignments)
        {
            List<(int, int)> create = new List<(int, int)>();
            List<int> remove = new List<int>();

            foreach (Function function in metadata.Functions)
            {
                IEnumerable<Barrier> barriers = assignments.Where(x => x.Value)
                    .Select(x => metadata.Barriers[x.Key])
                    .Where(x => x.BarrierType == "ordered")
                    .Where(x => function.FunctionName == x.Function);

                if (!barriers.Any())
                    continue;

                int min = barriers.Min(x => x.Location.Line);
                int max = barriers.Max(x => x.Location.Line);

                IEnumerable<DataRace> races = repairer.Races
                    .Where(x => x.Start >= function.Start.Line)
                    .Where(x => x.Start < min)
                    .Where(x => function.End == null || x.End <= function.End.Line);
                int? max2 = races.Max(x => x.End);

                races = repairer.Races.Where(x => x.Start == null || x.End == null);
                if (max2 == null && races.Any())
                {
                    max2 = metadata.Barriers.Values
                        .Where(x => x.BarrierType == "ordered")
                        .Where(x => function.FunctionName == x.Function)
                        .Max(x => x.Location.Line);
                }

                max = max2.HasValue ? Math.Max(max, max2.Value) : max;
                if (min == max)
                {
                    // an existing ordered region at the right place will be at line-1
                    bool check = metadata.Existing.Where(x => x.BarrierType == "ordered")
                        .Any(x => x.Location.Line == max-1);
                    if (check)
                        continue;
                }

                create.Add((min, max));
            }

            foreach (Barrier existing in metadata.Existing.Where(x => x.BarrierType == "ordered"))
            {
                bool keepExisting = true;
                if (!assignments.Any())
                    keepExisting = false;
                    
                foreach ((int, int) range in create)
                    if (existing.Location.Line >= range.Item1 && existing.Location.Line <= range.Item2)
                    {
                        keepExisting = false;
                        continue;
                    }

                if (!keepExisting)
                    remove.Add(existing.Location.Line);
            }

            create = create.OrderBy(x => x.Item1).ToList();
            remove = remove.Distinct().OrderBy(x => x).ToList();
            return (create, remove);
        }
    }
}

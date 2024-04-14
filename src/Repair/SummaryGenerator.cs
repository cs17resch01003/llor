namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using LLOR.Common;

    public class SummaryGenerator
    {
        private Repairer repairer;

        private Verifier verifier;

        private Metadata metadata;

        public SummaryGenerator(
            Verifier verifier, Repairer repairer, Metadata metadata)
        {
            this.repairer = repairer;
            this.verifier = verifier;
            this.metadata = metadata;
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
            (List<Location>, List<Location>) barriers = GenerateBarrierSummary(assignments);
            List<Location> add = barriers.Item1;
            List<Location> remove = barriers.Item2;

            bool barriers_check = (add.Any() || remove.Any()) && (
                add.Count == remove.Count ||
                add.Count - remove.Count >= metadata.Existing.Count(x => x.BarrierType == "barrier"));

            List<string> lines = new List<string>();
            foreach (Location location in add)
            {
                string sourceFile = new FileInfo(location.File).FullName;
                string fileDescription = includeFilename ? $" in {sourceFile}" : string.Empty;
                int line = location.Line;

                lines.Add($"Add a barrier at line number {line}{fileDescription}.");
            }

            foreach (Location location in remove)
            {
                string sourceFile = new FileInfo(location.File).FullName;
                string fileDescription = includeFilename ? $" in {sourceFile}" : string.Empty;
                int line = location.Line;

                lines.Add($"Remove the barrier at line number {line}{fileDescription}.");
            }

            return (barriers_check, lines);
        }

        private (List<Location>, List<Location>) GenerateBarrierSummary(Dictionary<string, bool> assignments)
        {
            List<Location> add = new List<Location>();
            List<Location> remove = new List<Location>();

            IEnumerable<Barrier> barriers = assignments.Where(x => x.Value)
                .Select(x => metadata.Barriers[x.Key])
                .Where(x => x.BarrierType == "barrier");
            foreach(Barrier barrier in barriers)
            {
                // an existing barrier at the right place will be at line-1
                bool check = !metadata.Existing.Where(x => x.BarrierType == "barrier")
                    .Any(x => x.Location.File == barrier.Location.File &&
                              x.Location.Line == barrier.Location.Line - 1);

                if (check)
                    add.Add(barrier.Location);
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
                    if (existing.Location.File == barrier.Location.File &&
                        existing.Location.Line == barrier.Location.Line - 1)
                    {
                        keepExisting = true;
                        break;
                    }
                }

                if (!keepExisting)
                {
                    bool check = false;
                    FileInfo file;

                    string path = Path.Combine(metadata.BasePath, existing.Location.File);
                    if (File.Exists(path))
                        file = new FileInfo(path);
                    else
                    {
                        file = new FileInfo(existing.Location.File);
                        if (!File.Exists(file.FullName))
                            continue;
                    }

                    string[] content = File.ReadAllLines(file.FullName);

                    string line = content[existing.Location.Line-1];
                    line = Regex.Replace(line, @"\s+", " ").Trim();

                    string extension = file.Extension;
                    string language = extension.Equals(".f95", StringComparison.InvariantCultureIgnoreCase) ? "Fortran" : "C";

                    // avoid removing implicit barriers
                    if ((language == "C" && line.Contains("pragma omp barrier")) ||
                        (language == "Fortran" && line.Contains("!$omp barrier")))
                    {
                        check = true;
                    }

                    if (check)
                        remove.Add(existing.Location);
                }
            }

            return (add, remove);
        }

        private (bool, List<string>) GenerateOrderedSummary(Dictionary<string, bool> assignments, bool includeFilename)
        {
            (List<(string, int, int)>, List<Location>) ordered = GenerateOrderedSummary(assignments);
            List<(string, int, int)> create = new List<(string, int, int)>();
            List<Location> remove = new List<Location>();

            foreach ((string, int, int) tuple in ordered.Item1)
            {
                bool exists = create.Any(x => x.Item1 == tuple.Item1 && x.Item2 == tuple.Item2 && x.Item3 == tuple.Item3);
                if (!exists)
                    create.Add(tuple);
            }

            foreach (Location location in ordered.Item2)
            {
                bool exists = remove.Any(x => x.File == location.File && x.Line == location.Line);
                if (!exists)
                    remove.Add(location);
            }

            create = create.OrderBy(x => x.Item1).ThenBy(x => x.Item2).ToList();
            remove = remove.OrderBy(x => x.File).ThenBy(x => x.Line).ToList();

            bool ordered_check = (create.Any() || remove.Any()) && (
                create.Count == remove.Count ||
                create.Count - remove.Count >= metadata.Existing.Count(x => x.BarrierType == "ordered"));

            List<string> lines = new List<string>();
            foreach ((string, int, int) range in create)
            {
                string sourceFile = new FileInfo(range.Item1).FullName;
                string fileDescription = includeFilename ? $" in {sourceFile}" : string.Empty;

                if (range.Item2 == range.Item3)
                    lines.Add($"Create an ordered region covering line {range.Item2}{fileDescription}.");
                else
                    lines.Add($"Create an ordered region covering lines {range.Item2} to {range.Item3}{fileDescription}.");
            }

            foreach (Location location in remove)
            {
                string sourceFile = new FileInfo(location.File).FullName;
                string fileDescription = includeFilename ? $" in {sourceFile}" : string.Empty;

                lines.Add($"Remove the ordered region at line number {location.Line}{fileDescription}.");
            }

            return (ordered_check, lines);
        }

        private (List<(string, int, int)>, List<Location>) GenerateOrderedSummary(Dictionary<string, bool> assignments)
        {
            List<(string, int, int)> create = new List<(string, int, int)>();
            List<Location> remove = new List<Location>();

            foreach (Function function in metadata.Functions)
            {
                IEnumerable<Barrier> barriers = assignments.Where(x => x.Value)
                    .Select(x => metadata.Barriers[x.Key])
                    .Where(x => x.BarrierType == "ordered")
                    .Where(x => x.Function == function.FunctionName)
                    .Where(x => x.Location.File == function.Start.File);

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
                        .Where(x => x.Function == function.FunctionName)
                        .Where(x => x.Location.File == function.Start.File)
                        .Max(x => x.Location.Line);
                }

                max = max2.HasValue ? Math.Max(max, max2.Value) : max;
                if (min == max)
                {
                    // an existing ordered region at the right place will be at line-1
                    bool check = metadata.Existing.Where(x => x.BarrierType == "ordered")
                        .Where(x => x.Location.File == function.Start.File)
                        .Any(x => x.Location.Line == max-1);
                    if (check)
                        continue;
                }

                create.Add((function.Start.File, min, max));
            }

            foreach (Barrier existing in metadata.Existing.Where(x => x.BarrierType == "ordered"))
            {
                bool keepExisting = true;
                if (!assignments.Any())
                    keepExisting = false;
                    
                foreach ((string, int, int) range in create)
                {
                    bool check = existing.Location.File == range.Item1 &&
                        existing.Location.Line >= range.Item2 && existing.Location.Line <= range.Item3;
                    if (check)
                    {
                        keepExisting = false;
                        continue;
                    }
                }

                if (!keepExisting)
                    remove.Add(existing.Location);
            }

            return (create, remove);
        }
    }
}

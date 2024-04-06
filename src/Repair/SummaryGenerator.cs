namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class SummaryGenerator
    {
        private string basePath;

        private string baseName;

        private Verifier verifier;

        private Metadata metadata;

        private FileInfo? sourceFile;

        private string? language;

        private string[]? fileContent;

        public SummaryGenerator(
            FileInfo inputFile, Verifier verifier, Metadata metadata, Options options)
        {
            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));
                
            basePath = inputFile.Directory.FullName;
            baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            this.verifier = verifier;
            this.metadata = metadata;

            sourceFile = verifier.GetSource(options);
            if (sourceFile != null)
            {
                string extension = sourceFile.Extension;
                language = extension.Equals(".f95", StringComparison.InvariantCultureIgnoreCase) ? "Fortran" : "C";
                fileContent = File.ReadAllLines(sourceFile.FullName);
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

            lines = lines.Distinct().OrderBy(x => x).ToList();
            return lines;
        }

        public void WriteSummary(IEnumerable<string> lines)
        {
            string summary_path = basePath + Path.DirectorySeparatorChar + baseName + ".summary";
            if (lines.Any())
                File.WriteAllLines(summary_path, lines.Distinct());
        }

        private void GenerateSummary(Dictionary<string, bool> assignments, List<string> lines, bool includeFilename)
        {
            Tuple<List<int>, List<int>> barriers = GenerateBarrierSummary(assignments);
            List<int> barriers_add = barriers.Item1;
            List<int> barriers_remove = barriers.Item2;

            bool barriers_check = (barriers_add.Any() || barriers_remove.Any()) && (
                barriers_add.Count == barriers_remove.Count ||
                barriers_add.Count - barriers_remove.Count >= metadata.Existing.Count(x => x.BarrierType == "barrier"));

            Tuple<List<Tuple<int, int>>, List<int>> ordered = GenerateOrderedSummary(assignments);
            List<Tuple<int, int>> ordered_create = ordered.Item1;
            List<int> ordered_remove = ordered.Item2;

            bool ordered_check = (ordered_create.Any() || ordered_remove.Any()) && (
                ordered_create.Count == ordered_remove.Count ||
                ordered_create.Count - ordered_remove.Count >= metadata.Existing.Count(x => x.BarrierType == "ordered"));

            if (barriers_check || ordered_check)
            {
                List<DataRace> races = verifier.VerifySource();
                if (!races.Any())
                    return;
            }

            string fileDescription = includeFilename && sourceFile != null ? $" in {sourceFile.FullName}" : string.Empty;
            foreach (int line in barriers_add)
                lines.Add($"Add a barrier at line number {line}{fileDescription}.");

            foreach (int line in barriers_remove)
                lines.Add($"Remove the barrier at line number {line}{fileDescription}.");

            foreach (Tuple<int, int> tuple in ordered_create)
            {
                if (tuple.Item1 == tuple.Item2)
                    lines.Add($"Create an ordered region covering line {tuple.Item1}{fileDescription}.");
                else
                    lines.Add($"Create an ordered region covering lines {tuple.Item1} to {tuple.Item2}{fileDescription}.");
            }

            foreach (int line in ordered_remove)
                lines.Add($"Remove the ordered region at line number {line}{fileDescription}.");
        }

        private Tuple<List<int>, List<int>> GenerateBarrierSummary(Dictionary<string, bool> assignments)
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
                bool check = !metadata.Existing
                    .Where(x => x.BarrierType == "barrier")
                    .Any(x => x.Location.Line == line-1);

                if (check)
                    add.Add(line);
            }

            IEnumerable<ExistingBarrier> existingBarriers =
                metadata.Existing.Where(x => x.BarrierType == "barrier");
            foreach (ExistingBarrier existing in existingBarriers)
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

            add = add.Distinct().ToList();
            remove = remove.Distinct().ToList();

            return new Tuple<List<int>, List<int>>(add, remove);
        }
        
        private Tuple<List<Tuple<int, int>>, List<int>> GenerateOrderedSummary(Dictionary<string, bool> assignments)
        {
            List<Tuple<int, int>> create = new List<Tuple<int, int>>();
            List<int> remove = new List<int>();

            IEnumerable<Barrier> barriers = assignments.Where(x => x.Value)
                .Select(x => metadata.Barriers[x.Key])
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
                        min = metadata.Barriers.Values
                            .Where(x => x.Function == function && x.Loop == loop)
                            .Min(x => x.Location.Line);
                        max = metadata.Barriers.Values
                            .Where(x => x.Function == function && x.Loop == loop)
                            .Max(x => x.Location.Line);
                    }

                    if (min == max)
                    {
                        // an existing ordered region at the right place will be at line-1
                        bool check = metadata.Existing
                            .Where(x => x.BarrierType == "ordered")
                            .Any(x => x.Location.Line == max-1);
                        
                        if (check)
                        {
                            continue;
                        }
                    }

                    bool exists = false;
                    foreach (Tuple<int, int> range in create)
                        if (range.Item1 == min && range.Item2 == max)
                            exists = true;

                    if (!exists)
                        create.Add(new Tuple<int, int>(min, max));
                }
            }

            IEnumerable<ExistingBarrier> existingBarriers =
                metadata.Existing.Where(x => x.BarrierType == "ordered");
            foreach (ExistingBarrier existing in existingBarriers)
            {
                bool keepExisting = false;
                barriers = assignments.Where(x => x.Value)
                    .Select(x => metadata.Barriers[x.Key])
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
                    remove.Add(existing.Location.Line);
            }

            remove = remove.Distinct().ToList();
            return new Tuple<List<Tuple<int, int>>, List<int>>(create, remove);
        }
    }
}

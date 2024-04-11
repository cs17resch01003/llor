namespace LLOR.TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using CommandLine;
    using CsvHelper;
    using CsvHelper.Configuration;
    using LLOR.Common;

    public class Program
    {
        public static void Main(string[] args)
        {
            Options options = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    options = o;
                });

            if (options == null) return;

            List<FileSystemInfo> paths = new List<FileSystemInfo>();
            foreach(string directory in Directory.EnumerateDirectories(options.FolderPath))
            {
                IEnumerable<string> files = Directory.EnumerateFiles(directory);
                if (files.Any())
                    foreach (string file in files.OrderBy(x => x))
                        paths.Add(new FileInfo(file));
                else
                    foreach (string folder in Directory.EnumerateDirectories(directory).OrderBy(x => x))
                        paths.Add(new DirectoryInfo(folder));
            }

            if (options.Verify)
                Verify(paths);
            else if (options.Repair)
                Repair(paths);
            else if (options.Check)
                Check(paths);
            else
                GenerateReport(paths, options.FolderPath);
        }

        private static void Verify(IEnumerable<FileSystemInfo> paths)
        {
            foreach (FileSystemInfo path in paths)
            {
                VerificationResult verify = Verify(path);

                string code = verify.StatusCode.ToString().ToLower();
                string sourcePath = path.FullName;

                string statement = $"{code}: {sourcePath}";
                Console.WriteLine(statement);
            }

            Console.WriteLine($"\n\nTotal: {paths.Count()}.");
        }

        private static VerificationResult Verify(FileSystemInfo path)
        {
            CommandOutput output = CommandRunner.RunCommand("llov", path.FullName);
            Output actual = new Output(
                (StatusCode)output.ExitCode,
                output.StandardError);

            ParsedOutput parsed = ErrorParser.ParseErrorOutput(actual.Result);
            StatusCode derived = parsed.StatusCode;

            if (derived == StatusCode.Pass)
                derived = actual.StatusCode;

            if (derived == StatusCode.Pass && parsed.Races.Any())
                derived = StatusCode.XFail;

            return new VerificationResult
            {
                StatusCode = derived,
                RaceCount = derived == StatusCode.XFail ? parsed.Races.Count : null,
            };
        }

        private static void Repair(IEnumerable<FileSystemInfo> paths)
        {
            int total = paths.Count(), success = 0;
            foreach (FileSystemInfo path in paths)
            {
                RepairResult repair = Repair(path);

                string assert = repair.Assert.ToString().ToLower();
                string code = repair.StatusCode == null ? string.Empty :
                    repair.StatusCode.ToString().ToLower();
                string filename = path.FullName;

                string statement = $"{assert}_{code}: {filename}";
                Console.WriteLine(statement);

                if (repair.Assert == true)
                    success++;
            }

            Console.WriteLine($"\n\nTotal: {total}. Success: {success}. Fail: {total-success}.");
        }

        private static RepairResult Repair(FileSystemInfo path, string? solverType = null)
        {
            RepairResult value = new RepairResult();
            value.Path = path.FullName;

            string arguments = $"--path {path.FullName} --testonly";
            if (solverType != null)
                arguments += " --solvertype " + solverType;

            Stopwatch watch = Stopwatch.StartNew();
            CommandOutput output = CommandRunner.RunCommand("llor", arguments);
            Output actual = new Output(
                (StatusCode)output.ExitCode,
                output.StandardOutput);
            value.TimeTaken = watch.ElapsedMilliseconds;

            Output expected = new Output(StatusCode.Pass);

            List<string> lines = new List<string>();
            string delimiter = "//;";

            FileInfo? file = path as FileInfo;
            if (file != null)
            {
                delimiter = file.Extension
                    .Equals(".f95", StringComparison.InvariantCultureIgnoreCase) ? "!;" : "//;";
                lines = File.ReadLines(path.FullName)
                    .Where(x => x.StartsWith(delimiter)).ToList();
            }
            else
            {
                string assert = Path.Combine(path.FullName, "assert.txt");
                lines = File.ReadLines(assert)
                    .Where(x => x.StartsWith(delimiter)).ToList();
            }

            bool result = false;
            if (lines.Any())
            {
                expected.StatusCode = Enum.Parse<StatusCode>(
                    lines[0].Replace(delimiter, string.Empty).Trim());

                lines.RemoveAt(0);
                expected.Result = lines.Where(x => x.StartsWith(delimiter))
                    .Select(x => x.Replace(delimiter, string.Empty).Trim()).ToList();

                if (actual.StatusCode == StatusCode.Pass)
                    result = expected.Equals(actual);
                else
                    result = expected.StatusCode == actual.StatusCode;
            }

            value.StatusCode = actual.StatusCode.ToString().ToLower();
            value.Assert = result;
            value.Lines = GetLines(path);

            foreach (string line in actual.Result)
            {
                if (line.StartsWith("Instructions"))
                    value.Instructions += int.Parse(line.Split(";")[1]);
                if (line.StartsWith("Barriers"))
                    value.Barriers += int.Parse(line.Split(";")[1]);
                if (line.StartsWith("Changes"))
                    value.Changes = int.Parse(line.Split(";")[1]);
                if (line.StartsWith("Watch"))
                {
                    string[] parts = line.Split(";");

                    if (parts[1] == "MaxSAT" || parts[1] == "Optimization")
                    {
                        value.SolverCount++;
                        value.SolverTime += int.Parse(parts[2]);
                    }
                    else if (parts[1] == "mhs")
                    {
                        value.MhsCount++;
                        value.MhsTime += int.Parse(parts[2]);
                    }
                    else if (parts[1] == "Verification")
                    {
                        value.VerificationCount++;
                        value.VerificationTime += int.Parse(parts[2]);
                    }
                    else if (parts[1] == "Total")
                    {
                        value.TotalTime = int.Parse(parts[2]);
                    }
                }
            }

            return value;
        }
    
        private static void Check(IEnumerable<FileSystemInfo> paths)
        {
            int total = paths.Count(), success = 0;
            foreach (FileSystemInfo path in paths)
            {
                FileSystemInfo fixedPath = GetFixedPath(path);

                VerificationResult verify = Verify(path);
                RepairResult repair = Repair(path);
                VerificationResult? repaired = Verify(fixedPath);

                string assert = repair.Assert.ToString().ToLower();
                string verifyCode = verify.StatusCode.ToString().ToLower();
                string repairCode = repair.StatusCode == null ? string.Empty :
                    repair.StatusCode.ToString().ToLower();
                string repairedCode = repaired == null ? "na" : repaired.StatusCode.ToString().ToLower();

                string statement = $"{assert}_{verifyCode}_{repairCode}_{repairedCode}: {path.FullName}";
                Console.WriteLine(statement);

                if (repair.Assert == true)
                    success++;
            }

            Console.WriteLine($"\n\nTotal: {total}. Success: {success}. Fail: {total-success}.");
        }

        private static FileSystemInfo GetFixedPath(FileSystemInfo path)
        {
            FileInfo? file = path as FileInfo;
            if (file != null)
            {
                if (file.DirectoryName == null)
                    throw new ArgumentNullException(nameof(file.DirectoryName));
                string fixedPath = Path.Combine(file.DirectoryName, "fixed", file.Name);
                return new FileInfo(fixedPath);
            }
            else
            {
                string fixedPath = Path.Combine(path.FullName, "fixed");
                return new DirectoryInfo(fixedPath);
            }
        }

        private static void GenerateReport(IEnumerable<FileSystemInfo> paths, string directory)
        {
            List<SourceCode> sourceCodes = new List<SourceCode>();
            List<Summary> summaries = new List<Summary>();

            List<RepairResult> mhs_results = new List<RepairResult>();
            List<RepairResult> maxsat_results = new List<RepairResult>();

            int total = paths.Count(), success = 0;
            foreach (FileSystemInfo path in paths)
            {
                VerificationResult verify = Verify(path);
                RepairResult mhs = Repair(path);
                RepairResult maxsat = Repair(path, "MaxSAT");

                sourceCodes.Add(new SourceCode
                {
                    Path = mhs.Path,
                    Lines = mhs.Lines,
                    Instructions = mhs.Instructions,
                    Barriers = mhs.Barriers,
                });

                summaries.Add(new Summary
                {
                    Path = mhs.Path,
                    VerificationResult = verify.StatusCode.ToString().ToLower(),
                    MhsResult = mhs.StatusCode,
                    MhsChanges = mhs.Changes,
                    MhsTimeTaken = mhs.TimeTaken,
                    MaxResult = maxsat.StatusCode,
                    MaxChanges = maxsat.Changes,
                    MaxTimeTaken = maxsat.TimeTaken,
                });

                string mhs_assert = mhs.Assert ? "PASS" : "FAIL";
                string statement = $"{mhs_assert}: {mhs.Path}";
                Console.WriteLine(statement);

                if (mhs.Assert == true)
                    success++;

                mhs_results.Add(mhs);
                maxsat_results.Add(maxsat);
            }

            Console.WriteLine($"\n\nTotal: {total}. Success: {success}. Fail: {total-success}.");

            CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.UTF8,
            };

            using (var writer = new StreamWriter(Path.Combine(directory, "summary.csv")))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(summaries);
            }

            using (var writer = new StreamWriter(Path.Combine(directory, "sourcecode.csv")))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(sourceCodes);
            }

            using (var writer = new StreamWriter(Path.Combine(directory, "mhs.csv")))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(mhs_results);
            }

            using (var writer = new StreamWriter(Path.Combine(directory, "maxsat.csv")))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(maxsat_results);
            }
        }
    
        private static int GetLines(FileSystemInfo path)
        {
            FileInfo? file = path as FileInfo;
            if (file != null)
                return GetLines(file);
            else
            {
                int lines = 0;
                foreach (string fileName in Directory.EnumerateFiles(path.FullName))
                {
                    file = new FileInfo(fileName);
                    bool check = file.Extension.Equals("c", StringComparison.InvariantCultureIgnoreCase)
                        || file.Extension.Equals("cpp", StringComparison.InvariantCultureIgnoreCase)
                        || file.Extension.Equals("h", StringComparison.InvariantCultureIgnoreCase);

                    if (check)
                        lines += GetLines(file);
                }

                return lines;
            }
        }

        private static int GetLines(FileInfo file)
        {
            int count = 0;

            string? line;
            using (StreamReader reader = File.OpenText(file.FullName))
            {
                do
                {
                    line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        count++;
                }
                while (line != null);
            }

            return count;
        }
    }
}

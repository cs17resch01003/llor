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

            IEnumerable<FileSystemInfo> paths = GetPaths(options);
            if (options.Verify)
                Verify(paths);
            else if (options.Repair)
                Repair(paths);
            else if (options.Check)
                Check(paths);
            else
                GenerateReport(paths, options.FolderPath);
        }

        private static IEnumerable<FileSystemInfo> GetPaths(Options options)
        {
            if (options.Type == "file")
            {
                IEnumerable<FileInfo> files = Directory.EnumerateFiles(options.FolderPath, "*.*", SearchOption.AllDirectories)
                    .Where(x => x.EndsWith(".c") || x.EndsWith(".cpp") || x.EndsWith(".f95"))
                    .OrderBy(x => x).Select(x => new FileInfo(x));
                return files.Where(x => !x.FullName.Contains("fixed")).ToList();
            }
            else
            {
                Stack<string> stack = new Stack<string>();
                stack.Push(options.FolderPath);

                List<DirectoryInfo> directories = new List<DirectoryInfo>();
                while (stack.Any())
                {
                    string directory = stack.Pop();
                    if (File.Exists(Path.Combine(directory, "Makefile")))
                        directories.Add(new DirectoryInfo(directory));
                    else
                        foreach (string temp in Directory.GetDirectories(directory))
                            stack.Push(temp);
                }
                
                return directories;
            }
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

            int timeout = 5*60*1000;

            string arguments = $"--path {path.FullName} --testonly";
            if (solverType != null)
                arguments += " --solvertype " + solverType;
            if (path is DirectoryInfo)
            {
                timeout = 24*60*60*1000;
                arguments += " --timeout 60";
            }

            Stopwatch watch = Stopwatch.StartNew();
            CommandOutput output = CommandRunner.RunCommand("llor", arguments, timeout);
            value.TimeTaken = watch.ElapsedMilliseconds;

            Output actual = PopulateActual(output, value);
            Output? expected = PopulateExpected(path);

            bool result = false;
            if (actual.StatusCode == StatusCode.Pass)
                result = expected != null ? expected.Equals(actual) : true;
            else
                result = expected != null ? expected.StatusCode == actual.StatusCode : true;

            value.Assert = result;
            value.Lines = GetLines(path);

            return value;
        }
    
        private static Output PopulateActual(CommandOutput output, RepairResult value)
        {
            Output actual = new Output(
                (StatusCode)output.ExitCode,
                output.StandardOutput);

            foreach (string line in actual.Result)
            {
                if (line.StartsWith("Instructions"))
                    value.Instructions += int.Parse(line.Split(";")[1]);
                else if (line.StartsWith("Barriers"))
                    value.Barriers += int.Parse(line.Split(";")[1]);
                else if (line.StartsWith("Changes"))
                    value.Changes = int.Parse(line.Split(";")[1]);
                else if (line.StartsWith("Watch"))
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
                else if (line.StartsWith("StatusCode"))
                {
                    string[] parts = line.Split(";");

                    StatusCode code = (StatusCode)Enum.Parse(typeof(StatusCode), parts[2]);
                    if (!value.Statuses.ContainsKey(code))
                        value.Statuses.Add(code, 0);

                    value.Statuses[code]++;
                }
            }

            value.StatusCode = actual.StatusCode.ToString().ToLower();
            int total = value.Statuses.Sum(x => x.Value);
            if (total > 1)
            {
                if (value.Statuses.Count == 1)
                {
                    actual.StatusCode = value.Statuses.Keys.First();
                }
                else if (value.Statuses.ContainsKey(StatusCode.Timeout))
                {
                    if (value.Statuses[StatusCode.Timeout] == total)
                        actual.StatusCode = StatusCode.Timeout;
                    else
                        actual.StatusCode = StatusCode.PartialTimeout;
                }
                else
                {
                    value.Statuses.Remove(StatusCode.Unsupported);
                    total = value.Statuses.Sum(x => x.Value);

                    if (value.Statuses.ContainsKey(StatusCode.Pass))
                    {
                        if (value.Statuses[StatusCode.Pass] == total)
                            actual.StatusCode = StatusCode.Pass;
                    }
                    else
                    {
                        if (value.Statuses.ContainsKey(StatusCode.RepairError))
                            actual.StatusCode = StatusCode.RepairError;
                        else if (value.Statuses.ContainsKey(StatusCode.Fail))
                            actual.StatusCode = StatusCode.Fail;
                        else
                            actual.StatusCode = StatusCode.Error;
                    }
                }
            }

            return actual;
        }

        private static Output? PopulateExpected(FileSystemInfo path)
        {
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

            if (lines.Any())
            {
                Output expected = new Output(StatusCode.Pass);
                expected.StatusCode = Enum.Parse<StatusCode>(
                    lines[0].Replace(delimiter, string.Empty).Trim());

                lines.RemoveAt(0);
                expected.Result = lines.Where(x => x.StartsWith(delimiter))
                    .Select(x => x.Replace(delimiter, string.Empty).Trim()).ToList();

                return expected;
            }

            return null;
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
                string statement = $"{DateTime.Now:HHmmss} {mhs_assert}: {mhs.Path}";
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

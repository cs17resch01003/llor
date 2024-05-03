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

            if (options == null || string.IsNullOrWhiteSpace(options.FolderPath)) return;

            IEnumerable<FileSystemInfo> benchmarks = GetBenchmarks(options);
            GenerateReport(benchmarks, options);
        }

        private static IEnumerable<FileSystemInfo> GetBenchmarks(Options options)
        {
            Stack<string> stack = new Stack<string>();
            stack.Push(options.FolderPath);

            List<FileSystemInfo> benchmarks = new List<FileSystemInfo>();
            while (stack.Any())
            {
                string directory = stack.Pop();
                if (File.Exists(Path.Combine(directory, "Makefile")))
                    benchmarks.Add(new DirectoryInfo(directory));
                else if (File.Exists(Path.Combine(directory, "make.common")))
                    continue;
                else
                {
                    IEnumerable<FileInfo> files = Directory.EnumerateFiles(directory, "*.*")
                        .Where(x => x.EndsWith(".c") || x.EndsWith(".cpp") || x.EndsWith(".f95"))
                        .OrderBy(x => x).Select(x => new FileInfo(x));

                    if (files.Any())
                        benchmarks.AddRange(files);
                    else
                        foreach (string temp in Directory.GetDirectories(directory))
                            stack.Push(temp);
                }
            }

            return benchmarks;
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

        private static RepairResult Repair(FileSystemInfo benchmark, Options options, string? solverType = null)
        {
            RepairResult value = new RepairResult();
            value.Path = benchmark.FullName;

            int timeout = options.SingleFileTimeout;

            string arguments = $"--path {benchmark.FullName} --testonly";
            if (solverType != null)
                arguments += " --solvertype " + solverType;
            if (benchmark is DirectoryInfo)
            {
                timeout = options.MultiFileTimeout * 30;
                arguments += " --timeout " + (options.MultiFileTimeout / 1000);
            }

            CommandOutput output = CommandRunner.RunCommand("llor", arguments, timeout);

            Output actual = PopulateActual(output, value, benchmark is DirectoryInfo);
            Output? expected = PopulateExpected(benchmark);

            bool result = false;
            if (actual.StatusCode == StatusCode.Pass)
                result = expected != null ? expected.Equals(actual) : true;
            else
                result = expected != null ? expected.StatusCode == actual.StatusCode : true;

            value.Assert = result;
            value.Lines = GetLines(benchmark);

            return value;
        }
    
        private static Output PopulateActual(CommandOutput output, RepairResult value, bool folder)
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

            if (folder == true)
            {
                int total = value.Statuses.Sum(x => x.Value);
                if (value.Statuses.Count == 1)
                {
                    actual.StatusCode = value.Statuses.Keys.First();
                }
                else if (value.Statuses.ContainsKey(StatusCode.Unsupported))
                {
                    actual.StatusCode = StatusCode.Unsupported;
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
                    if (value.Statuses.ContainsKey(StatusCode.RepairError))
                        actual.StatusCode = StatusCode.RepairError;
                    else if (value.Statuses.ContainsKey(StatusCode.Fail))
                        actual.StatusCode = StatusCode.Fail;
                    else if (value.Statuses.ContainsKey(StatusCode.Pass))
                    {
                        if (value.Statuses[StatusCode.Pass] == total)
                            actual.StatusCode = StatusCode.Pass;
                    }
                    else
                        actual.StatusCode = StatusCode.Error;
                }
            }

            value.StatusCode = actual.StatusCode.ToString().ToLower();
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
                if (File.Exists(assert))
                {
                    lines = File.ReadLines(assert)
                        .Where(x => x.StartsWith(delimiter)).ToList();
                }
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

        private static void GenerateReport(IEnumerable<FileSystemInfo> benchmarks, Options options)
        {
            List<Summary>[] array = new List<Summary>[options.Runs];
            for(int i = 0; i < options.Runs; i++)
                array[i] = RunExperiments(benchmarks, options);

            List<Summary> summaries = ConsolidateSummaries(array);
            CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ",",
                Encoding = Encoding.UTF8,
            };

            using (var writer = new StreamWriter(Path.Combine(options.FolderPath, "summary.csv")))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(summaries);
            }
        }

        private static List<Summary> RunExperiments(IEnumerable<FileSystemInfo> benchmarks, Options options)
        {
            List<Summary> summaries = new List<Summary>();
            Stopwatch watch = Stopwatch.StartNew();

            int total = benchmarks.Count(), success = 0;
            foreach (FileSystemInfo benchmark in benchmarks)
            {
                VerificationResult verify = Verify(benchmark);
                RepairResult mhs = Repair(benchmark, options);
                RepairResult maxsat = Repair(benchmark, options, "MaxSAT");

                if (verify.StatusCode == StatusCode.Pass && mhs.Changes != 0)
                    mhs.StatusCode = "passchange";
                if (verify.StatusCode == StatusCode.Pass && maxsat.Changes != 0)
                    maxsat.StatusCode = "passchange";

                summaries.Add(new Summary
                {
                    Path = mhs.Path,
                    Lines = mhs.Lines,
                    Instructions = mhs.Instructions,
                    Barriers = mhs.Barriers,
                    VerificationResult = verify.StatusCode.ToString().ToLower(),
                    MhsResult = mhs.StatusCode,
                    MhsTimeTaken = mhs.TotalTime,
                    MhsCount = mhs.MhsCount,
                    MhsSolverCount = mhs.SolverCount,
                    MaxResult = maxsat.StatusCode,
                    MaxTimeTaken = maxsat.TotalTime,
                    MaxSolverCount = maxsat.SolverCount,
                });

                string mhs_assert = mhs.Assert ? "PASS" : "FAIL";
                string statement = $"{mhs_assert}: {mhs.Path}";
                Console.WriteLine(statement);

                if (mhs.Assert == true)
                    success++;
            }

            Console.WriteLine($"\n\nTotal: {total}. Success: {success}. Fail: {total-success}.");
            Console.WriteLine($"Time Taken: {watch.ElapsedMilliseconds / 1000} seconds.");

            return summaries;
        }
    
        private static List<Summary> ConsolidateSummaries(List<Summary>[] array)
        {
            List<Summary> result = new List<Summary>();
            foreach (Summary first in array[0])
            {
                Summary summary = new Summary();

                List<Summary> summaries = new List<Summary>();
                foreach (List<Summary> temp in array)
                    summaries.Add(temp.First(x => x.Path == first.Path));

                summary.Path = first.Path;
                summary.Lines = summaries.Average(x => x.Lines);
                summary.Instructions = summaries.Average(x => x.Instructions);
                summary.Barriers = summaries.Average(x => x.Barriers);

                if (summaries.Select(x => x.VerificationResult).Distinct().Count() == 1)
                    summary.VerificationResult = summaries.Select(x => x.VerificationResult).Distinct().First();
                else
                {
                    Console.WriteLine($"Verification result mismatch: {summary.Path}");
                    summary.VerificationResult = first.VerificationResult;
                }

                if (summaries.Select(x => x.MhsResult).Distinct().Count() == 1)
                {
                    summary.MhsResult = summaries.Select(x => x.MhsResult).Distinct().First();
                    summary.MhsTimeTaken = summaries.Average(x => x.MhsTimeTaken);
                    summary.MhsCount = summaries.Average(x => x.MhsCount);
                    summary.MhsSolverCount = summaries.Average(x => x.MhsSolverCount);
                }
                else
                {
                    Console.WriteLine($"MhsRepair result mismatch: {summary.Path}");
                    summary.MhsResult = first.MhsResult;
                    summary.MhsTimeTaken = first.MhsTimeTaken;
                    summary.MhsCount = first.MhsCount;
                    summary.MhsSolverCount = first.MhsSolverCount;
                }

                if (summaries.Select(x => x.MaxResult).Distinct().Count() == 1)
                {
                    summary.MaxResult = summaries.Select(x => x.MaxResult).Distinct().First();
                    summary.MaxTimeTaken = summaries.Average(x => x.MaxTimeTaken);
                    summary.MaxSolverCount = summaries.Average(x => x.MaxSolverCount);
                }
                else
                {
                    Console.WriteLine($"MaxRepair result mismatch: {summary.Path}");
                    summary.MaxResult = first.MaxResult;
                    summary.MaxTimeTaken = first.MaxTimeTaken;
                    summary.MaxSolverCount = first.MaxSolverCount;
                }

                result.Add(summary);
            }

            return result;
        }

        private static int GetLines(FileSystemInfo path)
        {
            FileInfo? file = path as FileInfo;
            if (file != null)
                return GetLines(file);
            else
            {
                int lines = 0;
                foreach (string fileName in Directory.EnumerateFiles(path.FullName, "*.*", SearchOption.AllDirectories))
                {
                    file = new FileInfo(fileName);
                    bool check = file.Extension.Equals(".c", StringComparison.InvariantCultureIgnoreCase)
                        || file.Extension.Equals(".cc", StringComparison.InvariantCultureIgnoreCase)
                        || file.Extension.Equals(".cpp", StringComparison.InvariantCultureIgnoreCase)
                        || file.Extension.Equals(".h", StringComparison.InvariantCultureIgnoreCase)
                        || file.Extension.Equals(".hh", StringComparison.InvariantCultureIgnoreCase);

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

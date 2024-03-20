namespace LLOR.TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
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

            IEnumerable<FileInfo> files = Directory.EnumerateFiles(options.FolderPath, "*.*", SearchOption.AllDirectories)
                .Where(x => x.EndsWith(".c") || x.EndsWith(".cpp") || x.EndsWith(".f95"))
                .OrderBy(x => x).Select(x => new FileInfo(x));
            files = files.Where(x => !x.FullName.Contains("fixed"));

            if (options.Verify)
                Verify(files);
            else if (options.Repair)
                Repair(files);
            else if (options.Check)
                Check(files);
            else
                GenerateReport(files, options.FolderPath);
        }

        private static void Verify(IEnumerable<FileInfo> files)
        {
            foreach (FileInfo file in files)
            {
                VerificationResult verify = Verify(file);

                string code = verify.StatusCode.ToString().ToLower();
                string filename = file.FullName;

                string statement = $"{code}: {filename}";
                Console.WriteLine(statement);
            }

            Console.WriteLine($"\n\nTotal: {files.Count()}.");
        }

        private static VerificationResult Verify(FileInfo file)
        {
            CommandOutput output = CommandRunner.RunCommand("llov", file.FullName);
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

        private static void Repair(IEnumerable<FileInfo> files)
        {
            int total = files.Count(), success = 0;
            foreach (FileInfo file in files)
            {
                RepairResult repair = Repair(file);

                string assert = repair.Assert.ToString().ToLower();
                string code = repair.StatusCode.ToString().ToLower();
                string filename = file.FullName;

                string statement = $"{assert}_{code}: {filename}";
                Console.WriteLine(statement);

                if (repair.Assert == true)
                    success++;
            }

            Console.WriteLine($"\n\nTotal: {total}. Success: {success}. Fail: {total-success}.");
        }

        private static RepairResult Repair(FileInfo file, string? solverType = null)
        {
            if (file.DirectoryName == null)
                    throw new ArgumentNullException(nameof(file.DirectoryName));

            RepairResult value = new RepairResult();
            value.FilePath = Path.Combine(new DirectoryInfo(file.DirectoryName).Name, file.Name);

            string arguments = $"--file {file.FullName} --testonly";
            if (solverType != null)
                arguments += " --solvertype " + solverType;

            Stopwatch watch = Stopwatch.StartNew();
            CommandOutput output = CommandRunner.RunCommand("llor", arguments);
            Output actual = new Output(
                (StatusCode)output.ExitCode,
                output.StandardOutput);
            value.TimeTaken = watch.ElapsedMilliseconds;

            Output expected = new Output(StatusCode.Pass);
            string delimiter = file.Extension
                .Equals(".f95", StringComparison.InvariantCultureIgnoreCase) ? "!;" : "//;";

            List<string> lines = File.ReadLines(file.FullName)
                .Where(x => x.StartsWith(delimiter)).ToList();

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
            value.Lines = GetLines(file.FullName);

            foreach (string line in actual.Result)
            {
                if (line.StartsWith("Instructions"))
                    value.Instructions = int.Parse(line.Split(";")[1]);
                if (line.StartsWith("Barriers"))
                    value.Barriers = int.Parse(line.Split(";")[1]);
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
    
        private static void Check(IEnumerable<FileInfo> files)
        {
            int total = files.Count(), success = 0;
            foreach (FileInfo file in files)
            {
                if (file.DirectoryName == null)
                    throw new ArgumentNullException(nameof(file.DirectoryName));

                VerificationResult verify = Verify(file);
                RepairResult repair = Repair(file);
                VerificationResult? repaired = null;

                string fixedPath = Path.Combine(file.DirectoryName, "fixed", file.Name);
                if (File.Exists(fixedPath))
                    repaired = Verify(new FileInfo(fixedPath));

                string assert = repair.Assert.ToString().ToLower();
                string verifyCode = verify.StatusCode.ToString().ToLower();
                string repairCode = repair.StatusCode.ToString().ToLower();
                string repairedCode = repaired == null ? "na" : repaired.StatusCode.ToString().ToLower();
                string filename = file.FullName;

                string statement = $"{assert}_{verifyCode}_{repairCode}_{repairedCode}: {filename}";
                Console.WriteLine(statement);

                if (repair.Assert == true)
                    success++;
            }

            Console.WriteLine($"\n\nTotal: {total}. Success: {success}. Fail: {total-success}.");
        }

        private static void GenerateReport(IEnumerable<FileInfo> files, string directory)
        {
            List<Record> records = new List<Record>();
            List<RepairResult> mhs_results = new List<RepairResult>();
            List<RepairResult> maxsat_results = new List<RepairResult>();

            int total = files.Count(), success = 0;
            foreach (FileInfo file in files)
            {
                if (file.DirectoryName == null)
                    throw new ArgumentNullException(nameof(file.DirectoryName));

                VerificationResult verify = Verify(file);
                RepairResult mhs = Repair(file);
                RepairResult maxsat = Repair(file, "MaxSAT");

                Record record = new Record();
                record.FilePath = Path.Combine(new DirectoryInfo(file.DirectoryName).Name, file.Name);
                record.Lines = mhs.Lines;
                record.Instructions = mhs.Instructions;
                record.Barriers = mhs.Barriers;

                record.VerificationResult = verify.StatusCode.ToString().ToLower();

                record.MhsResult = mhs.StatusCode;
                record.MhsTimeTaken = mhs.TimeTaken;

                record.MaxResult = maxsat.StatusCode;
                record.MaxTimeTaken = maxsat.TimeTaken;

                string mhs_assert = mhs.Assert ? "PASS" : "FAIL";
                string max_assert = maxsat.Assert ? "PASS" : "FAIL";
                string statement = $"{mhs_assert}: {max_assert}: {record.FilePath}";
                Console.WriteLine(statement);

                if (mhs.Assert == true && maxsat.Assert == true)
                    success++;
                
                records.Add(record);
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

            using (var writer = new StreamWriter(Path.Combine(directory, "result.csv")))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(records);
            }

            using (var writer = new StreamWriter(Path.Combine(directory, "result_mhs.csv")))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(mhs_results);
            }

            using (var writer = new StreamWriter(Path.Combine(directory, "result_maxsat.csv")))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(maxsat_results);
            }
        }
    
        private static int GetLines(string file)
        {
            int count = 0;

            string? line;
            using (StreamReader reader = File.OpenText(file))
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

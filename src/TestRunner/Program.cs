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
                .Where(x => x.EndsWith(".c") || x.EndsWith(".cpp"))
                .OrderBy(x => x).Select(x => new FileInfo(x));

            if (options.Verify)
                Verify(files);
            else if (options.Repair)
                Repair(files);
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

        private static RepairResult Repair(FileInfo file)
        {
            Output expected = new Output(StatusCode.Pass);

            List<string> lines = File.ReadLines(file.FullName).ToList();
            expected.StatusCode = Enum.Parse<StatusCode>(
                lines[0].Replace("//;", string.Empty).Trim());

            lines.RemoveAt(0);
            expected.Result = lines.Where(x => x.StartsWith("//;"))
                .Select(x => x.Replace("//;", string.Empty).Trim()).ToList();
            
            string arguments = $"--file {file.FullName} --testonly";
            CommandOutput output = CommandRunner.RunCommand("llor", arguments);

            Output actual = new Output(
                (StatusCode)output.ExitCode,
                output.StandardOutput);

            bool result = false;
            if (actual.StatusCode == StatusCode.Pass)
                result = expected.Equals(actual);
            else
                result = expected.StatusCode == actual.StatusCode;

            return new RepairResult
            {
                StatusCode = actual.StatusCode,
                Changes = actual.Result.Count,
                Assert = result,
            };
        }
    
        private static void GenerateReport(IEnumerable<FileInfo> files, string directory)
        {
            List<Record> records = new List<Record>();

            int total = files.Count(), success = 0;
            foreach (FileInfo file in files)
            {
                Stopwatch watch = Stopwatch.StartNew();

                VerificationResult verify = Verify(file);
                RepairResult repair = Repair(file);

                string assert = repair.Assert.ToString().ToLower();
                string verifyCode = verify.StatusCode.ToString().ToLower();
                string repairCode = repair.StatusCode.ToString().ToLower();
                string filename = file.FullName;

                string statement = $"{assert}_{verifyCode}_{repairCode}: {filename}";
                Console.WriteLine(statement);

                if (repair.Assert == true)
                    success++;

                if (file.DirectoryName == null)
                    throw new ArgumentNullException(nameof(file.DirectoryName));

                records.Add(new Record
                {
                    FilePath = new DirectoryInfo(file.DirectoryName).Name + "\\" + file.Name,
                    VerificationResult = verifyCode,
                    RepairResult = repairCode,
                    Changes = repair.Changes,
                    TimeTaken = watch.ElapsedMilliseconds,
                });
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
        }
    }
}

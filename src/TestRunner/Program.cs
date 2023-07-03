namespace LLOR.TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CommandLine;
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

            DirectoryInfo directory = new DirectoryInfo(options.FolderPath);
            IEnumerable<FileInfo> files = directory.GetFiles()
                .Where(x => !x.Name.EndsWith(".summary")).OrderBy(x => x.Name);

            Parallel.ForEach(
                files,
                new ParallelOptions { MaxDegreeOfParallelism = options.Threads },
                file =>
                {
                    if (options.SummaryOnly)
                        RunRepair(options, file);
                    else if (options.Verify)
                        AssertVerify(options, file);
                    else
                        AssertRepair(options, file);
                });
        }

        private static void RunRepair(Options options, FileInfo file)
        {
            string arguments = $"--file {file.FullName} --summaryonly";
            CommandRunner.RunCommand(options.LLORBinariesPath, arguments);
        }

        private static void AssertVerify(Options options, FileInfo file)
        {
            string arguments = $"-Xclang -disable-O0-optnone -Xclang -load -Xclang "
                + $"{options.LLOVBinariesPath}/lib/OpenMPVerify.so "
                + $"-I{options.IncludePath} -fopenmp -g {file.FullName}";
                
            CommandOutput output = CommandRunner.RunCommand(
                $"{options.LLOVBinariesPath}/bin/clang", arguments);
            Output actual = new Output(
                (StatusCode)output.ExitCode,
                output.StandardError);

            ParsedOutput parsed = ErrorParser.ParseErrorOutput(actual.Result);
            StatusCode derived = parsed.StatusCode;
            if (derived == StatusCode.Pass)
                derived = actual.StatusCode;

            if (derived == StatusCode.Pass && parsed.Races.Any())
                derived = StatusCode.XFail;

            string statement = $"{derived.ToString().ToLower()}: {file.FullName} ";
            if (derived == StatusCode.XFail)
                statement += $"({parsed.Races.Count})";
            
            Console.WriteLine(statement);
        }

        private static void AssertRepair(Options options, FileInfo file)
        {
            Output expected = new Output(StatusCode.Pass);

            List<string> lines = File.ReadLines(file.FullName).ToList();
            expected.StatusCode = Enum.Parse<StatusCode>(
                lines[0].Replace("//;", string.Empty).Trim());

            lines.RemoveAt(0);
            expected.Result = lines.Where(x => x.StartsWith("//;"))
                .Select(x => x.Replace("//;", string.Empty).Trim()).ToList();
            
            string arguments = $"--file {file.FullName} --testonly";
            CommandOutput output =
                CommandRunner.RunCommand(options.LLORBinariesPath, arguments);

            Output actual = new Output(
                (StatusCode)output.ExitCode,
                output.StandardOutput);

            bool result = false;
            if (actual.StatusCode == StatusCode.Pass)
                result = expected.Equals(actual);
            else
                result = expected.StatusCode == actual.StatusCode;

            Console.WriteLine(
                $"{actual.StatusCode.ToString().ToLower()} ("
                + $"{result.ToString().ToLower()}): {file.FullName}");
        }
    }
}

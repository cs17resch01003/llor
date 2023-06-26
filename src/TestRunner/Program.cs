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
            IEnumerable<FileInfo> files = directory.GetFiles().OrderBy(x => x.Name);

            Parallel.ForEach(
                files,
                new ParallelOptions { MaxDegreeOfParallelism = options.Threads },
                file =>
                {
                    if (!options.VerifyOnly)
                        AssertRepair(options, file);
                    else
                        AssertVerify(options, file);
                });
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

            bool result = expected.Equals(actual);
            Console.WriteLine(
                (result ? "PASS" : "FAIL") + ": " + file.FullName);
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

            ParsedOutput parsed = ParseErrorOutput(actual.Result);
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

        private static ParsedOutput ParseErrorOutput(List<string> output)
        {
            ParsedOutput parsed = new ParsedOutput();
            List<ParseMessage> messages = new List<ParseMessage>
            {
                new ParseMessage("Region is Data Race Free.", 2, StatusCode.Pass),
                new ParseMessage("warning", 2, StatusCode.Pass),
                new ParseMessage("Tasking and DOACROSS loops are not supported.", 1, StatusCode.Unsupported),
                new ParseMessage("Directive Not Analyzed by the verifier.", 1, StatusCode.Unsupported),
                new ParseMessage("/usr/bin/ld", 0, StatusCode.Fail),
                new ParseMessage("undefined reference", 0, StatusCode.Fail),
            };

            DataRace? current = null;
            for(int i = 0; i < output.Count; i++)
            {
                string line = output.ElementAt(i);
                if (line == "Data Race detected.")
                {
                    current = new DataRace();
                    parsed.Races.Add(current);
                }
                else if (line.StartsWith("Source") || line.StartsWith("Sink"))
                {
                    if (current == null)
                    {
                        parsed.Unparsed.Add(line);
                        break;
                    }

                    string[] parts = line.Split(":");
                    if (line.StartsWith("Source"))
                        current.Source = new Location(int.Parse(parts[2]), int.Parse(parts[3]));
                    else
                        current.Sink = new Location(int.Parse(parts[2]), int.Parse(parts[3]));
                }
                else
                {
                    ParseMessage? message =
                        messages.FirstOrDefault(x => line.Contains(x.Message));
                    if (message != null)
                    {
                        i += message.Skip;
                        if (parsed.StatusCode == StatusCode.Pass)
                            parsed.StatusCode = message.StatusCode;
                    }
                    else
                        parsed.Unparsed.Add(line);
                }
            }

            return parsed;
        }
    }
}

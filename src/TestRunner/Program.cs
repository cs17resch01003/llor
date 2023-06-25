namespace TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommandLine;

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
            foreach (FileInfo file in directory.GetFiles())
            {
                if (!options.VerifyOnly)
                    AssertRepair(options, file);
                else
                    AssertVerify(options, file);
            }
        }

        private static void AssertRepair(Options options, FileInfo file)
        {
            Output expected = new Output(StatusCode.Pass);

            List<string> lines = File.ReadLines(file.FullName).ToList();
            expected.StatusCode = StatusCode.GetStatusCode(
                lines[0].Replace("//;", string.Empty).Trim());

            lines.RemoveAt(0);
            expected.Result = lines.Where(x => x.StartsWith("//;"))
                .Select(x => x.Replace("//;", string.Empty).Trim()).ToList();
            
            string arguments = $"--file {file.FullName} --testonly";
            Output actual =
                CommandRunner.RunCommand(options.LLORBinariesPath, arguments);

            bool result = expected.Equals(actual);
            Console.WriteLine(
                (result ? "PASS" : "FAIL") + ": " + file.FullName);
        }

        private static void AssertVerify(Options options, FileInfo file)
        {
            Output expected = new Output(StatusCode.Fail);

            string arguments = $"-Xclang -disable-O0-optnone -Xclang -load -Xclang "
                + $"../lib/OpenMPVerify.so -I{options.IncludePath} -fopenmp -g {file.FullName}";
            Output actual =
                CommandRunner.RunCommand(options.LLOVBinariesPath, arguments);

            bool result = expected.Equals(actual);
            Console.WriteLine(
                (result ? "PASS" : "FAIL") + ": " + file.FullName);
        }
    }
}

namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommandLine;
    using LLOR.Common;
    using LLOR.Common.Exceptions;
    using LLOR.Repair.Diagnostics;
    using LLOR.Repair.Exceptions;

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
            Logger.DetailedLogging = options.DetailedLogging;

            try
            {
                Verifier verifier = new Verifier(options.FilePath);
                Instrumentor instrumentor = new Instrumentor(options);
                Logger.Log($"Barriers;{instrumentor.Barriers.Count()}");

                Repairer repairer = new Repairer(verifier, instrumentor);
                Dictionary<string, bool> assignments = repairer.Repair(options.SolverType);

                SummaryGenerator generator = new SummaryGenerator(
                    options.FilePath,
                    instrumentor);
                IEnumerable<string> changes = generator.GenerateSummary(assignments);

                foreach (string change in changes)
                    Console.WriteLine(change);

                Logger.Log($"Changes;{changes.Count()}");
                CleanFiles(options, changes.Count());
            }
            catch (RepairException ex)
            {
                HandleException(options, ex.StatusCode, ex.Message);
            }
            catch (CommandLineException ex)
            {
                HandleException(options, ex.StatusCode, ex.Message);
            }
        }

        private static void HandleException(Options options, StatusCode statusCode, string message)
        {
            CleanFiles(options);
            Console.WriteLine(message);

            Environment.Exit((int)statusCode);
        }

        private static void CleanFiles(Options options, int? changes = null)
        {
            FileInfo inputFile = new FileInfo(options.FilePath);

            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));

            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            string inst_path = basePath + Path.DirectorySeparatorChar + baseName + ".inst.ll";
            string sb_path = basePath + Path.DirectorySeparatorChar + baseName + ".sb.ll";
            string summary_path = basePath + Path.DirectorySeparatorChar + baseName + ".summary";

            File.Delete(sb_path);
            if (!changes.HasValue || changes == 0 || options.TestOnly)
            {
                File.Delete(inst_path);
                File.Delete(summary_path);
            }
        }
    }
}

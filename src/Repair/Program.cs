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
            using (Watch watch = new Watch(Measure.Total))
            {
                Options options = new Options();
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(o =>
                    {
                        options = o;
                    });

                if (options == null) return;
                Logger.DetailedLogging = options.DetailedLogging;

                List<string> changes  = new List<string>();

                IEnumerable<FileInfo> files = Initializer.Initialize(options.Path);
                foreach (FileInfo file in files)
                {
                    try
                    {
                        Instrumentor instrumentor = new Instrumentor(file);
                        instrumentor.Instrument(options);
                        Logger.Log($"Barriers;{instrumentor.Metadata.Barriers.Count()}");

                        Verifier verifier = new Verifier(file);

                        Repairer repairer = new Repairer(verifier, instrumentor);
                        Dictionary<string, bool> assignments = repairer.Repair(options);

                        IEnumerable<string> temp = new List<string>();
                        if (assignments.Any(x => x.Value))
                        {
                            SummaryGenerator generator = new SummaryGenerator(
                                file, verifier, repairer, instrumentor.Metadata, options);
                            temp = generator.GenerateSummary(assignments, files.Count() > 1);
                            changes.AddRange(temp);

                            if (files.Count() == 1)
                                generator.WriteSummary(changes);
                        }

                        CleanFiles(file, options, temp.Count());
                    }
                    catch (RepairException ex)
                    {
                        HandleException(file, options, ex.StatusCode, ex.Message);
                    }
                    catch (CommandLineException ex)
                    {
                        HandleException(file, options, ex.StatusCode, ex.Message);
                    }
                }

                if (files.Count() != 1)
                    SummaryGenerator.WriteSummary(new DirectoryInfo(options.Path).FullName, changes);
                    
                Logger.Log($"Changes;{changes.Count()}");
                foreach (string change in changes)
                    Console.WriteLine(change);
            }
        }

        private static void HandleException(
            FileInfo inputFile, Options options, StatusCode statusCode, string message)
        {
            CleanFiles(inputFile, options);
            Console.WriteLine(message);

            Environment.Exit((int)statusCode);
        }

        private static void CleanFiles(FileInfo inputFile, Options options, int? changes = null)
        {
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

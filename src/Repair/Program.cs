namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken ct = source.Token;

                    List<string> file_changes = new List<string>();
                    bool completed = false;

                    Task task = Task.Run(() =>
                    {
                        file_changes = RepairFile(file, options, files.Count() == 1, ct, ref completed);
                    });

                    if (!task.Wait(options.Timeout * 1000))
                    {
                        source.Cancel();
                        while (!task.Wait(1000));
                    }

                    changes.AddRange(file_changes);
                }

                if (files.Count() != 1 && changes.Any())
                    SummaryGenerator.WriteSummary(new DirectoryInfo(options.Path).FullName, changes);
                    
                Logger.Log($"Changes;{changes.Count()}");
            }
        }

        private static List<string> RepairFile(FileInfo file, Options options, bool singleFile,
            CancellationToken ct, ref bool completed)
        {
            Instrumentor instrumentor = new Instrumentor(file, options);
            Verifier verifier = new Verifier(instrumentor.Metadata);
            Repairer repairer = new Repairer(verifier, instrumentor);

            List<string> changes = new List<string>();
            try
            {
                instrumentor.Instrument(options);
                Logger.Log($"Barriers;{instrumentor.Metadata.Barriers.Count()}");

                Dictionary<string, bool> assignments = repairer.Repair(options, ct);

                SummaryGenerator generator = new SummaryGenerator(verifier, repairer, instrumentor.Metadata);
                if (assignments.Any(x => x.Value) || instrumentor.Metadata.Existing.Any())
                    changes = generator.GenerateSummary(assignments, !singleFile).ToList();

                if (singleFile && changes.Any())
                    generator.WriteSummary(changes);

                CleanFiles(file, options, changes.Count());
            }
            catch (OperationCanceledException)
            {
                FileInfo? sourceFile = instrumentor.Metadata.SourceFile;
                string message = $"Repair of {sourceFile?.FullName} timed out.";

                HandleException(file, options, message, changes);
            }
            catch (RepairException ex)
            {
                if (singleFile)
                    PrintException(file, options, ex.StatusCode, ex.Message);
                else
                {
                    FileInfo? sourceFile = instrumentor.Metadata.SourceFile;
                    string message = $"Repair of {sourceFile?.FullName} failed.";

                    HandleException(file, options, message, changes);
                }
            }
            catch (VerificationException ex)
            {
                if (singleFile)
                    PrintException(file, options, ex.StatusCode, ex.Message);
                else
                {
                    FileInfo? sourceFile = instrumentor.Metadata.SourceFile;
                    string message = $"Verification of {sourceFile?.FullName} failed.";

                    HandleException(file, options, message, changes);
                }
            }
            catch (CommandLineException ex)
            {
                PrintException(file, options, ex.StatusCode, ex.Message);
            }

            foreach (string change in changes)
                Console.WriteLine(change);
            
            completed = true;
            return changes;
        }

        private static void HandleException(
            FileInfo inputFile, Options options, string message, List<string> changes)
        {
            CleanFiles(inputFile, options);
            changes.Add(message);
        }

        private static void PrintException(
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

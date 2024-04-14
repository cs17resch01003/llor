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

                    (StatusCode, List<string>) result = (StatusCode.Unsupported, new List<string>());
                    Task task = Task.Run(() =>
                    {
                        result = RepairFile(file, options, files.Count() == 1, ct);
                    });

                    if (!task.Wait(options.Timeout * 1000))
                    {
                        source.Cancel();
                        while (!task.Wait(1000));
                    }

                    changes.AddRange(result.Item2);
                    if (options.DetailedLogging)
                        Logger.Log($"StatusCode;{file.FullName};{result.Item1}");
                }

                if (files.Count() != 1 && changes.Any())
                    SummaryGenerator.WriteSummary(new DirectoryInfo(options.Path).FullName, changes);
                    
                Logger.Log($"Changes;{changes.Count()}");
            }
        }

        private static (StatusCode, List<string>) RepairFile(
            FileInfo file, Options options, bool singleFile, CancellationToken ct)
        {
            Instrumentor instrumentor = new Instrumentor(file, options);
            Verifier verifier = new Verifier(instrumentor.Metadata, file);
            Repairer repairer = new Repairer(verifier, instrumentor);

            List<string> changes = new List<string>();
            StatusCode status = StatusCode.Error;

            try
            {
                instrumentor.Instrument(options);
                Logger.Log($"Barriers;{instrumentor.Metadata.Barriers.Count()}");

                Dictionary<string, bool> assignments = repairer.Repair(options, ct);

                SummaryGenerator generator = new SummaryGenerator(verifier, repairer, instrumentor.Metadata, file);
                if (assignments.Any(x => x.Value) || instrumentor.Metadata.Existing.Any())
                    changes = generator.GenerateSummary(assignments, !singleFile).ToList();

                if (singleFile && changes.Any())
                    generator.WriteSummary(changes);

                CleanFiles(file, options, changes.Count());
                status = StatusCode.Pass;
            }
            catch (OperationCanceledException)
            {
                FileInfo? sourceFile = instrumentor.Metadata.SourceFile;
                string source = sourceFile == null ? file.FullName : sourceFile.FullName;
                string message = $"Repair of {source} timed out.";

                HandleException(file, options, message, changes);
                status = StatusCode.Timeout;
            }
            catch (RepairException ex)
            {
                status = StatusCode.RepairError;
                if (singleFile)
                    PrintException(file, options, status, ex.Message);
                else
                {
                    FileInfo? sourceFile = instrumentor.Metadata.SourceFile;
                    string source = sourceFile == null ? file.FullName : sourceFile.FullName;
                    string message = $"Repair of {source} failed.";

                    HandleException(file, options, message, changes);
                }
            }
            catch (UnsupportedException ex)
            {
                status = StatusCode.Unsupported;
                if (singleFile)
                    PrintException(file, options, status, ex.Message);
                else
                {
                    FileInfo? sourceFile = instrumentor.Metadata.SourceFile;
                    string source = sourceFile == null ? file.FullName : sourceFile.FullName;
                    string message = $"Repair of {source} is unsupported.";

                    HandleException(file, options, message, changes);
                }
            }
            catch (VerificationException ex)
            {
                status = StatusCode.Fail;
                if (singleFile)
                    PrintException(file, options, status, ex.Message);
                else
                {
                    FileInfo? sourceFile = instrumentor.Metadata.SourceFile;
                    string source = sourceFile == null ? file.FullName : sourceFile.FullName;
                    string message = $"Verification of {source} failed.";

                    HandleException(file, options, message, changes);
                }
            }
            catch (CommandLineException ex)
            {
                status = StatusCode.Error;
                PrintException(file, options, status, ex.Message);
            }

            foreach (string change in changes)
                Console.WriteLine(change);

            return (status, changes);
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

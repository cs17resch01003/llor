namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommandLine;
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

            try
            {
                Verifier verifier = new Verifier(options.FilePath);
                Instrumentor instrumentor = new Instrumentor(options.FilePath);
                    
                Repairer repairer = new Repairer(verifier, instrumentor);
                Dictionary<string, bool> assignments = repairer.Repair();

                SummaryGenerator generator = new SummaryGenerator(
                    options.FilePath,
                    instrumentor);
                IEnumerable<string> changes = generator.GenerateSummary(assignments);

                foreach (string change in changes)
                    Console.WriteLine(change);

                CleanFiles(options, changes.Count());
            }
            catch (RepairException ex)
            {
                Console.WriteLine(ex.Message);
                CleanFiles(options);

                Environment.Exit((int)ex.StatusCode);
            }
        }

        private static void CleanFiles(Options options, int? changes = null)
        {
            FileInfo inputFile = new FileInfo(options.FilePath);

            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));

            string sourcePath = inputFile.FullName;
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

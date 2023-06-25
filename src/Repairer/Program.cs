namespace LLOR
{
    using CommandLine;

    public class Program
    {
        public static void Main(String[] args)
        {
            Options options = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    options = o;
                });

            if (options == null) return;
            
            Verifier verifier = new Verifier(
                options.BinariesPath, 
                options.IncludePath,
                options.FilePath);
            verifier.Initialize();

            Instrumentor instrumentor = new Instrumentor(
                options.BinariesPath,
                options.FilePath);
            Repairer repairer = new Repairer(verifier, instrumentor);

            Dictionary<string, bool> assignments = repairer.Repair();

            SummaryGenerator generator = new SummaryGenerator(
                options.FilePath,
                instrumentor);
            IEnumerable<string> changes = generator.GenerateSummary(assignments);

            foreach (string change in changes)
                Console.WriteLine(change);

            if (options.TestOnly)
                CleanFiles(options.FilePath);
        }

        private static void CleanFiles(string inputPath)
        {
            FileInfo inputFile = new FileInfo(inputPath);

            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));

            string sourcePath = inputFile.FullName;
            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            string sb_path = basePath + Path.DirectorySeparatorChar + baseName + ".sb.ll";
            string inst_path = basePath + Path.DirectorySeparatorChar + baseName + ".inst.ll";
            string summary_path = basePath + Path.DirectorySeparatorChar + baseName + ".summary";

            File.Delete(sb_path);
            File.Delete(inst_path);
            File.Delete(summary_path);
        }
    }
}

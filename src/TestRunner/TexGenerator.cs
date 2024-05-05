namespace LLOR.TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class TexGenerator
    {
        public static void Generate(List<Summary> summaries, Options options)
        {
            IEnumerable<string> files = Directory.EnumerateFiles("template", "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string destination = file.Replace("template", options.FolderPath + Path.DirectorySeparatorChar + "report");
                CopyFile(file, destination);
            }

            PrepareBenchmarkSummary(summaries, options);
            PrepareExperimentalResults(summaries, options);
        }

        private static void CopyFile(string source, string destination)
        {
            FileInfo info = new FileInfo(destination);
            if (info.Directory == null)
                throw new ArgumentNullException(nameof(info.Directory));

            if (!info.Directory.Exists)
                info.Directory.Create();

            File.Copy(source, destination);
        }

        private static void PrepareBenchmarkSummary(List<Summary> summaries, Options options)
        {
            string file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "benchmark_summary.tex";

            string content = File.ReadAllText(file);
            content = content.Replace("@@total@@", summaries, "program was", "programs were");
            content = content.Replace("@@cplusplus@@",
                summaries.Where(x => x.Path != null && !x.Path.EndsWith(".f95")));
            content = content.Replace("@@fortran@@",
                summaries.Where(x => x.Path != null && x.Path.EndsWith(".f95")));

            content = content.Replace("@@sftimeout@@", options.SingleFileTimeout / 1000, "second", "seconds");
            content = content.Replace("@@mfftimeout@@", options.MultiFileTimeout / 1000, "second", "seconds");
            content = content.Replace("@@mftimeout@@", options.MultiFileTimeout * 30 / 1000, "second", "seconds");
            content = content.Replace("@@runs@@", options.Runs, "time", "times");
            
            File.WriteAllText(file, content);

            file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "tables" +
                Path.DirectorySeparatorChar + "benchmark_summary.tex";

            content = File.ReadAllText(file);
            content = content.Replace("@@sf_c@@",
                summaries.Where(x => x.Path != null && !x.Path.EndsWith(".f95") && x.Source is FileInfo));
            content = content.Replace("@@sf_f@@",
                summaries.Where(x => x.Path != null && x.Path.EndsWith(".f95") && x.Source is FileInfo));
            content = content.Replace("@@sf_t@@",
                summaries.Where(x => x.Source is FileInfo));

            content = content.Replace("@@mf_c@@",
                summaries.Where(x => x.Path != null && !x.Path.EndsWith(".f95") && x.Source is DirectoryInfo));
            content = content.Replace("@@mf_f@@",
                summaries.Where(x => x.Path != null && x.Path.EndsWith(".f95") && x.Source is DirectoryInfo));
            content = content.Replace("@@mf_t@@",
                summaries.Where(x => x.Source is DirectoryInfo));

            File.WriteAllText(file, content);
        }

        private static void PrepareExperimentalResults(List<Summary> summaries, Options options)
        {
            string file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "experimental_results.tex";

            string content = File.ReadAllText(file);
            content = content.Replace("@@changes@@",
                summaries.Where(x => x.VerificationResult == "pass" && x.MhsResult == "passchange"),
                "program", "programs");

            IEnumerable<Summary> xfail = summaries.Where(x => x.VerificationResult == "xfail");
            content = content.Replace("@@xfail@@", xfail, "program", "programs");
            content = content.Replace("@@repaired@@",
                xfail.Where(x => x.MhsResult == "pass"), "program", "programs");
            content = content.Replace("@@repairerror@@",
                xfail.Where(x => x.MhsResult == "repairerror"), "program", "programs");
            content = content.Replace("@@unsupported@@",
                xfail.Where(x => x.MhsResult == "unsupported"), "program", "programs");
            content = content.Replace("@@timedout@@",
                xfail.Where(x => x.MhsResult == "timeout" || x.MhsResult == "partialtimeout"), "program", "programs");

            IEnumerable<Summary> unsupported = summaries.Where(x => x.VerificationResult != "pass" && x.VerificationResult != "xfail");
            content = content.Replace("@@llovunsupported@@", unsupported, "program", "programs");
            content = content.Replace("@@llorunsupported@@",
                unsupported.Where(x => x.VerificationResult == "unsupported" && x.MhsResult == "unsupported"),
                "program", "programs");
            content = content.Replace("@@lloverrors@@",
                unsupported.Where(x => x.VerificationResult != "unsupported"),
                "program", "programs");

            content = content.Replace("@@total@@", summaries, "benchmark", "benchmarks");

            IEnumerable<Summary> usedsolver = summaries.Where(x => x.MhsCount > 0 || x.MhsSolverCount > 0 || x.MaxSolverCount > 0)
                .Where(x => x.MhsResult != "timeout" && x.MhsResult != "partialtimeout");
            content = content.Replace("@@usedsolver@@", usedsolver, "benchmark", "benchmarks");
            content = content.Replace("@@mhsbetter@@",
                usedsolver.Where(x => x.MhsTimeTaken <= x.MaxTimeTaken),
                "benchmark", "benchmarks");
        }
    }
}

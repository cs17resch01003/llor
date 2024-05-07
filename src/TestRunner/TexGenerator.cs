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
            PrepareSourceInformation(summaries, options);
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

            File.WriteAllText(file, content);

            PrepareResults(summaries, options);
            PrepareTimeData(summaries, options);
        }

        private static void PrepareResults(List<Summary> summaries, Options options)
        {
            string file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "tables" +
                Path.DirectorySeparatorChar + "results.tex";

            string content = File.ReadAllText(file);
            IEnumerable<Summary> benchmarks_c = summaries.Where(x => x.Path != null && !x.Path.EndsWith(".f95"));
            IEnumerable<Summary> benchmarks_f = summaries.Where(x => x.Path != null && x.Path.EndsWith(".f95"));

            content = content.Replace("@@total_c@@", benchmarks_c);
            content = content.Replace("@@total_f@@", benchmarks_f);

            IEnumerable<Summary> pass_c = benchmarks_c.Where(x => x.VerificationResult == "pass");
            IEnumerable<Summary> pass_f = benchmarks_f.Where(x => x.VerificationResult == "pass");

            content = content.Replace("@@pass_c@@", pass_c);
            content = content.Replace("@@pass_f@@", pass_f);

            content = content.Replace("@@nochange_c@@", pass_c.Where(x => x.MhsResult == "pass"));
            content = content.Replace("@@nochange_f@@", pass_f.Where(x => x.MhsResult == "pass"));

            content = content.Replace("@@passchange_c@@", pass_c.Where(x => x.MhsResult == "passchange"));
            content = content.Replace("@@passchange_f@@", pass_f.Where(x => x.MhsResult == "passchange"));

            IEnumerable<Summary> race_c = benchmarks_c.Where(x => x.VerificationResult == "xfail");
            IEnumerable<Summary> race_f = benchmarks_f.Where(x => x.VerificationResult == "xfail");

            content = content.Replace("@@race_c@@", race_c);
            content = content.Replace("@@race_f@@", race_f);

            content = content.Replace("@@repaired_c@@", race_c.Where(x => x.MhsResult == "pass"));
            content = content.Replace("@@repaired_f@@", race_f.Where(x => x.MhsResult == "pass"));

            content = content.Replace("@@repairerror_c@@", race_c.Where(x => x.MhsResult == "repairerror"));
            content = content.Replace("@@repairerror_f@@", race_f.Where(x => x.MhsResult == "repairerror"));

            content = content.Replace("@@timeout_c@@", race_c.Where(x => x.MhsResult == "timeout" || x.MhsResult == "partialtimeout"));
            content = content.Replace("@@timeout_f@@", race_f.Where(x => x.MhsResult == "timeout" || x.MhsResult == "partialtimeout"));

            content = content.Replace("@@llorunsupported_c@@", race_c.Where(x => x.MhsResult == "unsupported"));
            content = content.Replace("@@llorunsupported_f@@", race_f.Where(x => x.MhsResult == "unsupported"));

            IEnumerable<Summary> unsupported_c = benchmarks_c.Where(x => x.VerificationResult != "pass" && x.VerificationResult != "xfail");
            IEnumerable<Summary> unsupported_f = benchmarks_f.Where(x => x.VerificationResult != "pass" && x.VerificationResult != "xfail");

            content = content.Replace("@@unsupported_c@@", unsupported_c);
            content = content.Replace("@@unsupported_f@@", unsupported_f);

            content = content.Replace("@@llovunsupported_c@@", unsupported_c.Where(x => x.VerificationResult == "unsupported" && x.MhsResult == "unsupported"));
            content = content.Replace("@@llovunsupported_f@@", unsupported_f.Where(x => x.VerificationResult == "unsupported" && x.MhsResult == "unsupported"));

            content = content.Replace("@@errors_c@@", unsupported_c.Where(x => x.VerificationResult != "unsupported"));
            content = content.Replace("@@errors_f@@", unsupported_f.Where(x => x.VerificationResult != "unsupported"));

            File.WriteAllText(file, content);
        }

        private static void PrepareTimeData(List<Summary> summaries, Options options)
        {
            string file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "figures" +
                Path.DirectorySeparatorChar + "data" +
                Path.DirectorySeparatorChar + "time_mhs_maxsat.dat";

            IEnumerable<Summary> usedsolver = summaries.Where(x => x.MhsCount > 0 || x.MhsSolverCount > 0 || x.MaxSolverCount > 0)
                .Where(x => x.MhsResult != "timeout" && x.MhsResult != "partialtimeout");
            int min = ((int)(Math.Floor(usedsolver.Min(x => x.MhsTimeTaken)) * 0.9)) / 100 * 100;
            int max = ((int)(Median(usedsolver.Select(x => x.MhsTimeTaken)) * 2.2)) / 100 * 100;

            string content = File.ReadAllText(file);
            IEnumerable<string> data = usedsolver.Select(x => string.Join("\t", new string[]
                {
                    (x.MhsTimeTaken > max ? max : (int)x.MhsTimeTaken).ToString(),
                    (x.MaxTimeTaken > max ? max : (int)x.MaxTimeTaken).ToString(),
                    "a"
                }));
            content = content.Replace("@@data@@", string.Join(Environment.NewLine, data));

            File.WriteAllText(file, content);

            file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "figures" +
                Path.DirectorySeparatorChar + "solvertime.tex";
            content = File.ReadAllText(file);

            content = content.Replace("@@min@@", min, false);
            content = content.Replace("@@max@@", max, false);

            File.WriteAllText(file, content);
        }

        private static void PrepareSourceInformation(List<Summary> summaries, Options options)
        {
            string file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "source_information.tex";
            string content = File.ReadAllText(file);

            IEnumerable<double> lines = summaries.Where(x => x.Lines.HasValue)
                .Select(x => x.Lines.HasValue ? x.Lines.Value : 0);
            content = content.Replace("@@lines_avg@@", lines.Average(), 2);
            content = content.Replace("@@lines_med@@", Median(lines));
            content = content.Replace("@@lines_100@@", lines.Where(x => x > 100),
                "program has", "programs have");
            content = content.Replace("@@lines_50@@", lines.Where(x => x > 50),
                "program has", "programs have");

            IEnumerable<double> insts = summaries.Where(x => x.Instructions.HasValue)
                .Select(x => x.Instructions.HasValue ? x.Instructions.Value : 0);
            content = content.Replace("@@insts_cnt@@", insts.Count(),
                "program", "programs");
            content = content.Replace("@@insts_fail@@", summaries.Count() - insts.Count(),
                "program", "programs");
            content = content.Replace("@@insts_avg@@", insts.Average(), 2);
            content = content.Replace("@@inst_med@@", Median(insts));

            IEnumerable<double> barriers = summaries.Where(x => x.Barriers.HasValue)
                .Select(x => x.Barriers.HasValue ? x.Barriers.Value : 0);
            content = content.Replace("@@bars_avg@@", barriers.Average(), 2);
            content = content.Replace("@@bars_med@@", Median(barriers));

            File.WriteAllText(file, content);

            PrepareSourceGraphs(summaries, options);
        }

        private static void PrepareSourceGraphs(List<Summary> summaries, Options options)
        {
            string file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "figures" +
                Path.DirectorySeparatorChar + "lines_insts.tex";
            string content = File.ReadAllText(file);

            IEnumerable<double> lines = summaries.Where(x => x.Lines.HasValue)
                .Select(x => x.Lines.HasValue ? x.Lines.Value : 0);
            content = content.Replace("@@lines_100@@", lines.Count(x => x > 100), false);
            content = content.Replace("@@lines_80@@", lines.Count(x => x >= 81 && x <= 100), false);
            content = content.Replace("@@lines_60@@", lines.Count(x => x >= 61 && x <= 80), false);
            content = content.Replace("@@lines_40@@", lines.Count(x => x >= 41 && x <= 60), false);
            content = content.Replace("@@lines_20@@", lines.Count(x => x >= 21 && x <= 40), false);
            content = content.Replace("@@lines_0@@", lines.Count(x => x <= 20), false);

            IEnumerable<double> insts = summaries.Where(x => x.Instructions.HasValue)
                .Select(x => x.Instructions.HasValue ? x.Instructions.Value : 0);
            content = content.Replace("@@insts_400@@", insts.Count(x => x > 400), false);
            content = content.Replace("@@insts_300@@", insts.Count(x => x >= 301 && x <= 400), false);
            content = content.Replace("@@insts_200@@", insts.Count(x => x >= 201 && x <= 300), false);
            content = content.Replace("@@insts_100@@", insts.Count(x => x >= 101 && x <= 200), false);
            content = content.Replace("@@insts_50@@", insts.Count(x => x >= 51 && x <= 100), false);
            content = content.Replace("@@insts_0@@", insts.Count(x => x <= 50), false);

            File.WriteAllText(file, content);

            file = options.FolderPath + Path.DirectorySeparatorChar + "report" +
                Path.DirectorySeparatorChar + "figures" +
                Path.DirectorySeparatorChar + "barriers.tex";
            content = File.ReadAllText(file);

            IEnumerable<double> barriers = summaries.Where(x => x.Barriers.HasValue)
                .Select(x => x.Barriers.HasValue ? x.Barriers.Value : 0);
            content = content.Replace("@@barrs_30@@", barriers.Count(x => x > 30), false);
            content = content.Replace("@@barrs_10@@", barriers.Count(x => x >= 11 && x <= 30), false);
            content = content.Replace("@@barrs_5@@", barriers.Count(x => x >= 6 && x <= 10), false);
            content = content.Replace("@@barrs_4@@", barriers.Count(x => x >= 4 && x <= 5), false);
            content = content.Replace("@@barrs_1@@", barriers.Count(x => x >= 1 && x <= 3), false);
            content = content.Replace("@@barrs_0@@", barriers.Count(x => x <= 0), false);

            File.WriteAllText(file, content);
        }

        private static double Median(IEnumerable<double> values)
        {
            int count = values.Count();
            int index = count / 2;

            if (count == 0)
                return 0;

            IEnumerable<double> sorted = values.OrderBy(n => n);
            double median = count % 2 != 0 ? sorted.ElementAt(index) :
                (sorted.ElementAt(index) + sorted.ElementAt(index - 1)) / 2;

            return median;
        }
    }
}

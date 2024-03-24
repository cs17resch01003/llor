namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using LLOR.Common;
    using LLOR.Repair.Diagnostics;
    using LLOR.Repair.Exceptions;
    
    public class Verifier
    {
        private FileInfo inputFile;

        public Verifier(string filepath)
        {
            inputFile = new FileInfo(filepath);
            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));

            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);
            string extension = inputFile.Extension;

            // generate <input>.ll
            string sourcePath = inputFile.FullName;
            string ll_path = basePath + Path.DirectorySeparatorChar + baseName + ".ll";
            string arguments = $"-fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone {sourcePath} -o {ll_path}";
            string command = extension.Equals(".f95", StringComparison.InvariantCultureIgnoreCase) ? "flang" : "clang";
            RunCommand(command, arguments);
            
            // flang has constructs that are not supported by llov
            if (extension.Equals(".f95", StringComparison.InvariantCultureIgnoreCase))
                Transformer.TransformIR(ll_path);

            // generate <input>.ssa.ll
            string ssa_path = basePath + Path.DirectorySeparatorChar + baseName + ".ssa.ll";
            arguments = $"-mem2reg -loop-simplify -simplifycfg {ll_path} -S -o {ssa_path}";
            RunCommand("opt", arguments);

            // generate <input>.rb.ll
            string rb_path = basePath + Path.DirectorySeparatorChar + baseName + ".rb.ll";
            arguments = $"-load OpenMPVerify.so -openmp-resetbounds {ssa_path} -S -o {rb_path}";
            RunCommand("opt", arguments);

            // generate <input>.in.ll
            string in_path = basePath + Path.DirectorySeparatorChar + baseName + ".in.ll";
            arguments = $"-load OpenMPVerify.so -openmp-forceinline -inline {rb_path} -S -o {in_path}";
            RunCommand("opt", arguments);

            // generate <input>.sb.ll
            string sb_path = basePath + Path.DirectorySeparatorChar + baseName + ".sb.ll";
            arguments = $"-load OpenMPVerify.so -openmp-split-basicblock {in_path} -S -o {sb_path}";
            RunCommand("opt", arguments);

            foreach(string path in new string[] { ll_path, ssa_path, rb_path, in_path })
                File.Delete(path);
        }
    
        public List<DataRace> Verify()
        {
            using (Watch watch = new Watch(Measure.Verification))
            {
                List<DataRace> races = new List<DataRace>();
                if (inputFile.Directory == null)
                    return races;

                string basePath = inputFile.Directory.FullName;
                string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

                // verify <input>.inst.ll
                string inst_path = basePath + Path.DirectorySeparatorChar + baseName + ".inst.ll";
                string arguments = $"-load OpenMPVerify.so -disable-output -openmp-verify-mhp {inst_path}";

                CommandOutput output = CommandRunner.RunCommand("opt", arguments);
                return GetDataRaces(output);
            }
        }

        public List<DataRace> VerifySource()
        {
            using (Watch watch = new Watch(Measure.Verification))
            {
                List<DataRace> races = new List<DataRace>();
                if (inputFile.Directory == null)
                    return races;

                string basePath = inputFile.Directory.FullName;
                string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

                // verify <input>.sb.ll
                string sb_path = basePath + Path.DirectorySeparatorChar + baseName + ".sb.ll";
                string arguments = $"-load OpenMPVerify.so -disable-output -openmp-verify-mhp {sb_path}";

                CommandOutput output = CommandRunner.RunCommand("opt", arguments);
                return GetDataRaces(output);
            }
        }

        public void ValidateSource()
        {
            string language = inputFile.Extension == ".f95" ? "Fortran" : "C";
            string section = language == "C" ? "#pragma omp section" : "!$omp section";
            string simd = language == "C" ? "#pragma omp simd" : "!$omp simd";

            List<string> lines = File.ReadLines(inputFile.FullName).ToList();
            foreach (string line in lines)
            {
                string temp = Regex.Replace(line, @"\s+", " ").Trim();
                if (language != "C" || !temp.StartsWith("//"))
                {
                    if (temp.Contains(section))
                        throw new RepairException(StatusCode.Unsupported, "Data races across sections cannot be repaired!");
                    else if (temp.Contains(simd))
                        throw new RepairException(StatusCode.Unsupported, "Data races inside a simd section cannot be repaired!");
                }
            }
        }

        private void RunCommand(string command, string arguments)
        {
            CommandOutput output = CommandRunner.RunCommand(command, arguments);
            if (output.ExitCode != 0)
                throw new RepairException(StatusCode.Fail, string.Join("\n", output.StandardError));
        }

        private List<DataRace> GetDataRaces(CommandOutput output)
        {
            List<DataRace> races = new List<DataRace>();
            ParsedOutput result = ErrorParser.ParseErrorOutput(output.StandardError);

            if (result.StatusCode == StatusCode.Pass)
            {
                foreach (Common.DataRace race in result.Races)
                    races.Add(new DataRace
                    {
                       Source = race.Source,
                       Sink = race.Sink, 
                    });

                return races;
            }
            else
            {
                throw new RepairException(
                    result.StatusCode,
                    string.Join('\n', output.StandardError));
            }
        }
    }
}

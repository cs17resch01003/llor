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
        private string basePath;

        private string baseName;

        public Verifier(FileInfo inputFile)
        {
            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));
                
            basePath = inputFile.Directory.FullName;
            baseName = Path.GetFileNameWithoutExtension(inputFile.Name);
        }
    
        public List<DataRace> Verify()
        {
            using (Watch watch = new Watch(Measure.Verification))
            {
                List<DataRace> races = new List<DataRace>();

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

                // verify <input>.sb.ll
                string sb_path = basePath + Path.DirectorySeparatorChar + baseName + ".sb.ll";
                string arguments = $"-load OpenMPVerify.so -disable-output -openmp-verify-mhp {sb_path}";

                CommandOutput output = CommandRunner.RunCommand("opt", arguments);
                return GetDataRaces(output);
            }
        }

        public void ValidateSource(Options options)
        {
            FileInfo? inputFile = GetSource(options);
            if (inputFile == null)
                return;

            string extension = inputFile.Extension;
            string language = extension == ".f95" ? "Fortran" : "C";
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

        public FileInfo? GetSource(Options options)
        {
            FileInfo? inputFile = null;
            if (File.Exists(options.Path))
            {
                inputFile = new FileInfo(options.Path);
            }
            else
            {
                string source_path = basePath + Path.DirectorySeparatorChar + baseName + ".c";
                if (File.Exists(source_path))
                    inputFile = new FileInfo(source_path);
                
                source_path = basePath + Path.DirectorySeparatorChar + baseName + ".cpp";
                if (File.Exists(source_path))
                    inputFile = new FileInfo(source_path);

                source_path = basePath + Path.DirectorySeparatorChar + baseName + ".h";
                if (File.Exists(source_path))
                    inputFile = new FileInfo(source_path);
            }

            return inputFile;
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

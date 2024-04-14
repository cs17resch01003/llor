namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using LLOR.Common;
    using LLOR.Common.Exceptions;
    using LLOR.Repair.Diagnostics;
    using LLOR.Repair.Exceptions;
    
    public class Verifier
    {
        private Metadata metadata;

        private string basePath;

        public Verifier(Metadata metadata, FileInfo inputFile)
        {
            this.metadata = metadata;

            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));

            basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);
            basePath = basePath + Path.DirectorySeparatorChar + baseName;
        }
    
        public List<DataRace> Verify(string extension = "inst")
        {
            try
            {
                using (Watch watch = new Watch(Measure.Verification))
                {
                    List<DataRace> races = new List<DataRace>();

                    // verify <input>.inst.ll
                    string path = basePath + $".{extension}.ll";
                    string arguments = $"-load OpenMPVerify.so -disable-output -openmp-verify-mhp {path}";

                    CommandOutput output = CommandRunner.RunCommand("opt", arguments);
                    return GetDataRaces(output);
                }
            }
            catch (CommandLineException ex)
            {
                throw new VerificationException(StatusCode.Fail, ex.Message);
            }
        }

        public List<DataRace> VerifySource()
        {
            return Verify("sb");
        }

        public void ValidateSource()
        {
            FileInfo? inputFile = metadata.SourceFile;
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
                        throw new UnsupportedException(StatusCode.Unsupported, "Data races across sections cannot be repaired!");
                    else if (temp.Contains(simd))
                        throw new UnsupportedException(StatusCode.Unsupported, "Data races inside a simd section cannot be repaired!");
                }
            }
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

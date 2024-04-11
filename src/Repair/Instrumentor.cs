namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using LLOR.Common;
    using LLOR.Repair.Diagnostics;
    using LLOR.Repair.Exceptions;
    
    public class Instrumentor
    {
        public Metadata Metadata = new Metadata();
        
        private string basePath;

        private string baseName;

        public Instrumentor(FileInfo inputFile)
        {
            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));
                
            basePath = inputFile.Directory.FullName;
            baseName = Path.GetFileNameWithoutExtension(inputFile.Name);
        }

        public void Instrument(Options options)
        {
            string extension = string.Empty;
            if (File.Exists(options.Path))
            {
                extension = new FileInfo(options.Path).Extension;
            }

            // generate <input>.inst.ll
            string sb_path = basePath + Path.DirectorySeparatorChar + baseName + ".sb.ll";
            string inst_path = basePath + Path.DirectorySeparatorChar + baseName + ".inst.ll";

            string arguments = $"-load OpenMPRepair.so -openmp-repair {sb_path} -S -o {inst_path} -initialize";
            if (extension.Equals(".f95", StringComparison.InvariantCultureIgnoreCase))
                arguments += " -language=Fortran";
            if (options.DetailedLogging)
                arguments += " -detailedlogging";

            CommandOutput output = CommandRunner.RunCommand("opt", arguments);

            if (output.ExitCode != (int)StatusCode.Pass)
            {
                throw new RepairException(
                    (StatusCode)output.ExitCode,
                    string.Join('\n', output.StandardError));
            }

            foreach (string line in output.StandardError)
            {
                if (line.StartsWith("Instructions"))
                {
                    Logger.Log(line);
                    continue;
                }

                string[] parts = line.Split(',');
                if (parts[0] == "existing")
                {
                    Metadata.Existing.Add(new Barrier(
                        parts[1],
                        int.Parse(parts[2]),
                        int.Parse(parts[3])
                    ));
                }
                if (parts[0] == "function")
                {
                    Metadata.Functions.Add(new Function(
                        parts[1],
                        int.Parse(parts[2]),
                        parts.Length == 4 ? int.Parse(parts[3]) : null));
                }
                else
                {
                    Metadata.Barriers.Add(parts[0], new Barrier(
                        parts[0],
                        parts[1],
                        int.Parse(parts[2]),
                        int.Parse(parts[3]),
                        parts[4]
                    ));
                }
            }
        }

        public void Update(Dictionary<string, bool> assignments)
        {
            string inst_path = basePath + Path.DirectorySeparatorChar + baseName + ".inst.ll";
            ResetBarriers(inst_path);
            UpdateBarriers(inst_path, assignments);

            string arguments = $"-load OpenMPRepair.so -openmp-repair {inst_path} -S -o {inst_path}";
            CommandOutput output = CommandRunner.RunCommand("opt", arguments);

            if (output.ExitCode != (int)StatusCode.Pass)
            {
                throw new RepairException(
                    (StatusCode)output.ExitCode,
                    string.Join('\n', output.StandardError));
            }
        }

        private void ResetBarriers(string filePath)
        {
            // by default the barriers are disabled
            foreach (Barrier barrier in Metadata.Barriers.Values)
                barrier.Enabled = false;
            UpdateBarriers(filePath);
        }

        private void UpdateBarriers(string filePath, Dictionary<string, bool> assignments)
        {
            foreach (string barrierName in assignments.Keys)
                Metadata.Barriers[barrierName].Enabled = assignments[barrierName];
            UpdateBarriers(filePath);
        }

        private void UpdateBarriers(string filePath)
        {
            List<string> result = new List<string>();

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string output = line;
                if (Regex.IsMatch(output, "!\"b\\d+\\.(barrier|ordered)\\.(true|false)\""))
                {
                    Match match = Regex.Match(output,
                        "!\"(?<barrierName>b\\d+?)\\.(?<barrierType>(barrier|ordered))\\.(?<enabled>(true|false)?)\"");

                    string barrierName = match.Groups["barrierName"].Value;
                    string barrierType = match.Groups["barrierType"].Value;
                    bool enabled = bool.Parse(match.Groups["enabled"].Value);

                    bool value = Metadata.Barriers[barrierName].Enabled;
                    if (enabled != value)
                    {
                        output = output.Replace(
                            $"!\"{barrierName}.{barrierType}.{enabled.ToString().ToLowerInvariant()}\"",
                            $"!\"{barrierName}.{barrierType}.{value.ToString().ToLowerInvariant()}\"");
                    }
                }

                result.Add(output);
            }

            File.WriteAllLines(filePath, result);
        }
    }
}

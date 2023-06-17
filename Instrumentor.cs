using System.Text.RegularExpressions;

namespace LLOR
{
    public class Instrumentor
    {
        private FileInfo inputFile;

        private string optPath;

        private string instrumentationPath;

        public Dictionary<string, Barrier> Barriers { get; set; } = new Dictionary<string, Barrier>();

        public List<Location> Existing { get; set; } = new List<Location>();

        public Instrumentor(string llovPath, string inputFile)
        {
            this.inputFile = new FileInfo(inputFile);

            DirectoryInfo directory = new DirectoryInfo(llovPath);
            string binPath = directory.FullName + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar;
            string libPath = directory.FullName + Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar;

            optPath = binPath + "opt";
            instrumentationPath = libPath + "OpenMPRepair.so";

            Instrument();
        }

        private void Instrument()
        {
            string sourcePath = inputFile.FullName;
            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            // generate <input>.inst.ll
            string sb_path = basePath + Path.DirectorySeparatorChar + baseName + ".sb.ll";
            string inst_path = basePath + Path.DirectorySeparatorChar + baseName + ".inst.ll";
            string command = optPath;
            string arguments = $"-load {instrumentationPath} -openmp-repair {sb_path} -S -o {inst_path} -initialize";
            List<string> result = CommandLine.RunCommand(command, arguments);

            foreach (string line in result)
            {
                string[] parts = line.Split(',');
                if (parts[0] == "existing")
                {
                    Existing.Add(new Location(
                        int.Parse(parts[1]),
                        int.Parse(parts[2])
                    ));
                }
                else
                {
                    Barriers.Add(parts[0], new Barrier(
                        parts[0],
                        int.Parse(parts[1]),
                        int.Parse(parts[2])
                    ));
                }
            }
        }

        public void Update(Dictionary<string, bool> assignments)
        {
            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            string inst_path = basePath + Path.DirectorySeparatorChar + baseName + ".inst.ll";
            ResetBarriers(inst_path);
            UpdateBarriers(inst_path, assignments);

            string command = optPath;
            string arguments = $"-load {instrumentationPath} -openmp-repair {inst_path} -S -o {inst_path}";
            CommandLine.RunCommand(command, arguments);
        }

        private void ResetBarriers(string filePath)
        {
            // by default the barriers are disabled
            foreach (Barrier barrier in Barriers.Values)
                barrier.Enabled = false;

            UpdateBarriers(filePath);
        }

        private void UpdateBarriers(string filePath, Dictionary<string, bool> assignments)
        {
            foreach (string barrierName in assignments.Keys)
                Barriers[barrierName].Enabled = assignments[barrierName];
            UpdateBarriers(filePath);
        }

        private void UpdateBarriers(string filePath)
        {
            List<string> result = new List<string>();

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string output = line;
                if (Regex.IsMatch(output, @"!b\d+\.(true|false)"))
                {
                    Match match = Regex.Match(output, @"!(?<barrierName>b\d+?)\.(?<enabled>(true|false)?)");

                    string barrierName = match.Groups["barrierName"].Value;
                    bool enabled = bool.Parse(match.Groups["enabled"].Value);
                    bool value = Barriers[barrierName].Enabled;

                    if (enabled != value)
                    {
                        output = output.Replace(
                            $"!{barrierName}.{enabled.ToString().ToLowerInvariant()}",
                            $"!{barrierName}.{value.ToString().ToLowerInvariant()}");
                    }
                }

                result.Add(output);
            }

            File.WriteAllLines(filePath, result);
        }
    }
}

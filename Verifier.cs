namespace LLOR
{
    using LLOR.Exceptions;
    
    public class Verifier
    {
        private FileInfo inputFile;

        private string clangPath, optPath, includePath;

        private string verifierPath, instrumentationPath;

        public Verifier(string llovPath, string includePath, string inputFile)
        {
            this.inputFile = new FileInfo(inputFile);
            this.includePath = new DirectoryInfo(includePath).FullName;

            DirectoryInfo directory = new DirectoryInfo(llovPath);
            string binPath = directory.FullName + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar;
            string libPath = directory.FullName + Path.DirectorySeparatorChar + "lib" + Path.DirectorySeparatorChar;

            clangPath = binPath + "clang";
            optPath = binPath + "opt";

            verifierPath = libPath + "OpenMPVerify.so";
            instrumentationPath = libPath + "OpenMPRepair.so";
        }

        public void Initialize()
        {
            if (inputFile.Directory == null)
                return;

            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            // generate <input>.ll
            string sourcePath = inputFile.FullName;
            string ll_path = basePath + Path.DirectorySeparatorChar + baseName + ".ll";
            string command = clangPath;
            string arguments = $"-fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone {sourcePath} -I{includePath} -o {ll_path}";
            CommandLine.RunCommand(command, arguments);

            // generate <input>.ssa.ll
            string ssa_path = basePath + Path.DirectorySeparatorChar + baseName + ".ssa.ll";
            command = optPath;
            arguments = $"-mem2reg -loop-simplify -simplifycfg {ll_path} -S -o {ssa_path}";
            CommandLine.RunCommand(command, arguments);

            // generate <input>.rb.ll
            string rb_path = basePath + Path.DirectorySeparatorChar + baseName + ".rb.ll";
            command = optPath;
            arguments = $"-load {verifierPath} -openmp-resetbounds {ssa_path} -S -o {rb_path}";
            CommandLine.RunCommand(command, arguments);

            // generate <input>.in.ll
            string in_path = basePath + Path.DirectorySeparatorChar + baseName + ".in.ll";
            command = optPath;
            arguments = $"-load {verifierPath} -openmp-forceinline -inline {rb_path} -S -o {in_path}";
            CommandLine.RunCommand(command, arguments);

            // generate <input>.sb.ll
            string sb_path = basePath + Path.DirectorySeparatorChar + baseName + ".sb.ll";
            command = optPath;
            arguments = $"-load {verifierPath} -openmp-split-basicblock {in_path} -S -o {sb_path}";
            CommandLine.RunCommand(command, arguments);

            foreach(string path in new string[] { ll_path, ssa_path, rb_path, in_path })
                File.Delete(path);
        }
    
        public List<DataRace> Verify()
        {
            List<DataRace> races = new List<DataRace>();
            if (inputFile.Directory == null)
                return races;

            string basePath = inputFile.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            // generate <input>.sb.ll
            string inst_path = basePath + Path.DirectorySeparatorChar + baseName + ".inst.ll";
            string command = optPath;
            string arguments = $"-load {verifierPath} -disable-output -openmp-verify-mhp {inst_path}";
            List<string> result = CommandLine.RunCommand(command, arguments);

            DataRace? current = null;
            foreach(string line in result)
            {
                if (line.StartsWith("Source") || line.StartsWith("Sink"))
                {
                    string[] parts = line.Split(":");
                    if (line.StartsWith("Source"))
                    {
                        current = new DataRace();
                        current.Source = new Location(int.Parse(parts[2]), int.Parse(parts[3]));
                    }
                    else
                    {
                        if (current == null)
                            throw new ExecutionException(result);

                        current.Sink = new Location(int.Parse(parts[2]), int.Parse(parts[3]));
                        races.Add(current);
                    }
                }
            }

            return races;
        }
    }
}

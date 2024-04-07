namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using LLOR.Common;
    using LLOR.Repair.Exceptions;
    
    public static class Initializer
    {
        public static IEnumerable<FileInfo> Initialize(string path)
        {
            FileSystemInfo input;
            if (Directory.Exists(path))
                input = new DirectoryInfo(path);
            else if (File.Exists(path))
                input = new FileInfo(path);
            else
                throw new InvalidDataException(nameof(path));

            string arguments = input.FullName;
            RunCommand("llov_compile", arguments);

            if (input is FileInfo)
            {
                FileInfo? inputFile = input as FileInfo;
                if (inputFile == null)
                    throw new ArgumentNullException(nameof(inputFile));

                if (inputFile.Directory == null)
                    throw new ArgumentNullException(nameof(inputFile.Directory));

                Transform(inputFile);

                string basePath = inputFile.Directory.FullName;
                string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

                string ll_path = basePath + Path.DirectorySeparatorChar + baseName + ".ll";
                return new List<FileInfo> { new FileInfo(ll_path) };
            }
            else
            {
                DirectoryInfo? inputDirectory = input as DirectoryInfo;
                if (inputDirectory == null)
                    throw new ArgumentNullException(nameof(inputDirectory));

                return Transform(inputDirectory);
            }
        }

        private static void Transform(FileInfo inputFile)
        {
            string arguments = inputFile.FullName;
            RunCommand("llov_transform", arguments);
        }

        private static IEnumerable<FileInfo> Transform(DirectoryInfo inputDirectory)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (FileInfo inputFile in inputDirectory.EnumerateFiles("*.ll"))
            {
                Transform(inputFile);
                files.Add(inputFile);
            }

            return files;
        }

        private static void RunCommand(string command, string arguments)
        {
            CommandOutput output = CommandRunner.RunCommand(command, arguments);
            if (output.ExitCode != 0)
                throw new RepairException(StatusCode.Fail, string.Join("\n", output.StandardError));
        }
    }
}
namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Metadata
    {
        public FileInfo? SourceFile { get; set; }

        public string BasePath { get; set; } = string.Empty;

        public string BaseName { get; set; } = string.Empty;
        
        public Dictionary<string, Barrier> Barriers { get; set; } = new Dictionary<string, Barrier>();

        public List<Barrier> Existing { get; set; } = new List<Barrier>();

        public List<Function> Functions { get; set; } = new List<Function>();

        public void PopulateData(FileInfo inputFile, Options options)
        {
            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));
                
            BasePath = inputFile.Directory.FullName;
            BaseName = Path.GetFileNameWithoutExtension(inputFile.Name);

            if (File.Exists(options.Path))
            {
                SourceFile = new FileInfo(options.Path);
            }
            else
            {
                foreach (string filePath in Directory.EnumerateFiles(BasePath, "*.*", SearchOption.AllDirectories))
                {
                    FileInfo file = new FileInfo(filePath);
                    List<string> extensions = new List<string> { ".c", ".cpp", ".h" };
                    foreach (string extension in extensions)
                    {
                        if (file.Name == BaseName + extension)
                        {
                            SourceFile = file;
                            return;
                        }
                    }
                }
            }
        }
    }
}

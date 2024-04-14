namespace LLOR.Repair
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Metadata
    {
        public FileInfo? SourceFile { get; set; }
        
        public Dictionary<string, Barrier> Barriers { get; set; } = new Dictionary<string, Barrier>();

        public List<Barrier> Existing { get; set; } = new List<Barrier>();

        public List<Function> Functions { get; set; } = new List<Function>();

        public void PopulateData(FileInfo inputFile, Options options)
        {
            if (inputFile.Directory == null)
                throw new ArgumentNullException(nameof(inputFile.Directory));
            
            if (File.Exists(options.Path))
            {
                SourceFile = new FileInfo(options.Path);
            }
            else
            {
                string basePath = new DirectoryInfo(options.Path).FullName;
                string baseName = Path.GetFileNameWithoutExtension(inputFile.Name);

                foreach (string filePath in Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories))
                {
                    FileInfo file = new FileInfo(filePath);
                    List<string> extensions = new List<string> { ".c", ".cpp", ".h" };
                    foreach (string extension in extensions)
                    {
                        if (file.Name.ToLower() == (baseName + extension).ToLower())
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

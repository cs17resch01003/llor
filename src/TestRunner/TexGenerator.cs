namespace LLOR.TestRunner
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class TexGenerator
    {
        public static void Generate(List<Summary> summaries, string directory)
        {
            IEnumerable<string> files = Directory.EnumerateFiles("template", "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string destination = file.Replace("template", directory + Path.DirectorySeparatorChar + "report");
                CopyFile(file, destination);
            }
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
    }
}

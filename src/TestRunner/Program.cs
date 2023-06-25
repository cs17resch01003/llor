namespace TestRunner
{
    using CommandLine;
    using System.IO;

    public class Program
    {
        public static void Main(String[] args)
        {
            Options options = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    options = o;
                });

            if (options == null) return;

            DirectoryInfo directory = new DirectoryInfo(options.FolderPath);
            foreach (FileInfo file in directory.GetFiles())
            {
                Console.WriteLine(file.FullName);
            }
        }
    }
}

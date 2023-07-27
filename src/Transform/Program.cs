namespace LLOR.Transform
{
    using CommandLine;
    using LLOR.Common;

    public class Program
    {
        public static void Main(string[] args)
        {
            Options options = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    options = o;
                });

            if (options == null) return;
            Transformer.TransformIR(options.FilePath);
        }
    }
}

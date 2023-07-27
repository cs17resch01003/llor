namespace LLOR.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public static class Transformer
    {
        public static void TransformIR(string path)
        {
            List<string> result = new List<string>();
            int count = 1;

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string temp = line;
                if (line.StartsWith("!") && line.Contains("!DIGlobalVariable") && line.Contains("flags"))
                    temp = Regex.Replace(line, "flags: .*", $"name: \"temp-{count}\")");
                    
                result.Add(temp);
            }

            File.WriteAllLines(path, result);
        }
    }
}

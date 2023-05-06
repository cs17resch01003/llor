namespace LLOR
{
    using System.Diagnostics;
    using LLOR.Exceptions;

    public static class CommandLine
    {
        public static List<string> RunCommand(string command, string arguments)
        {
            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };

            Process process = new Process();
            process.StartInfo = info;
            process.Start();

            List<string> result = new List<string>();
            if (process.StandardError != null)
            {
                while (!process.StandardError.EndOfStream)
                {
                    string? line = process.StandardError.ReadLine();
                    if (line != null)
                        result.Add(line);
                }
            }

            process.WaitForExit();

            if (process.ExitCode != 0)
                throw new ExecutionException(result);
            return result;
        }
    }
}

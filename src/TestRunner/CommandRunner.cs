namespace TestRunner
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class CommandRunner
    {
        public static Output RunCommand(string command, string arguments)
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
            if (process.StandardOutput != null)
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    string? line = process.StandardOutput.ReadLine();
                    if (line != null)
                        result.Add(line);
                }
            }

            process.WaitForExit();
            return new Output(StatusCode.GetStatusCode(process.ExitCode), result);
        }
    }
}

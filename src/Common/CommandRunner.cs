namespace LLOR.Common
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class CommandRunner
    {
        public static CommandOutput RunCommand(string command, string arguments)
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

            CommandOutput output = new CommandOutput();
            process.WaitForExit();
            output.ExitCode = process.ExitCode;

            if (process.StandardOutput != null)
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    string? line = process.StandardOutput.ReadLine();
                    if (line != null)
                        output.StandardOutput.Add(line);
                }
            }

            if (process.StandardError != null)
            {
                while (!process.StandardError.EndOfStream)
                {
                    string? line = process.StandardError.ReadLine();
                    if (line != null)
                        output.StandardError.Add(line);
                }
            }

            return output;
        }
    }
}

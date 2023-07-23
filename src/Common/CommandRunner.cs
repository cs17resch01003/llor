namespace LLOR.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using LLOR.Common.Exceptions;

    public static class CommandRunner
    {
        public static CommandOutput RunCommand(
            string command, string arguments, int timeout = 300000)
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
            if (!process.WaitForExit(timeout))
            {
                process.Kill();
                
                output.ExitCode = (int)StatusCode.Timeout;
                return output;
            }

            output.ExitCode = process.ExitCode;
            output.StandardOutput = GetResult(process.StandardOutput);
            output.StandardError = GetResult(process.StandardError);

            if (!Enum.IsDefined(typeof(StatusCode), process.ExitCode))
            {
                string message = string.Join('\n', output.StandardOutput, output.StandardError);
                throw new CommandLineException(StatusCode.Error, message);
            }

            return output;
        }

        private static List<string> GetResult(StreamReader reader)
        {
            List<string> output = new List<string>();
            if (reader != null)
            {
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    if (line != null)
                        output.Add(line);
                }
            }

            return output;
        }
    }
}

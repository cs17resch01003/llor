namespace LLOR.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
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

            using (Process process = new Process())
            {
                process.StartInfo = info;

                List<string> stdout = new List<string>();
                List<string> stderr = new List<string>();

                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) => {
                        if (e.Data == null)
                            outputWaitHandle.Set();
                        else
                            stdout.Add(e.Data);
                    };

                    process.ErrorDataReceived += (sender, e) => {
                        if (e.Data == null)
                            errorWaitHandle.Set();
                        else
                            stderr.Add(e.Data);
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    CommandOutput output = new CommandOutput();
                    if (process.WaitForExit(timeout) &&
                        outputWaitHandle.WaitOne(timeout) && errorWaitHandle.WaitOne(timeout))
                    {
                        output.ExitCode = process.ExitCode;
                        output.StandardOutput = stdout;
                        output.StandardError = stderr;

                        if (!Enum.IsDefined(typeof(StatusCode), process.ExitCode))
                        {
                            string message = string.Join('\n', 
                                string.Join('\n', output.StandardOutput.ToArray()),
                                string.Join('\n', output.StandardError.ToArray()));
                            throw new CommandLineException(StatusCode.Error, message);
                        }
                    }
                    else
                    {
                        process.Kill();
                        output.ExitCode = (int)StatusCode.Timeout;
                    }

                    return output;
                }
            }
        }
    }
}

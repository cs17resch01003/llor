namespace LLOR.Common
{
    using System.Collections.Generic;

    public class CommandOutput
    {
        public int ExitCode { get; set; }

        public List<string> StandardOutput { get; set; } = new List<string>();

        public List<string> StandardError { get; set; } = new List<string>();
    }
}

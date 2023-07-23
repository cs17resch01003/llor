namespace LLOR.Common.Exceptions
{
    using System;
    
    public class CommandLineException : Exception
    {
        public StatusCode StatusCode { get; set; }

        public CommandLineException(StatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

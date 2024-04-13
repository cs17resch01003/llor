namespace LLOR.Repair.Exceptions
{
    using System;
    using LLOR.Common;
    
    public class VerificationException : Exception
    {
        public StatusCode StatusCode { get; set; }

        public VerificationException(StatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

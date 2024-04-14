namespace LLOR.Repair.Exceptions
{
    using System;
    using LLOR.Common;
    
    public class UnsupportedException : Exception
    {
        public StatusCode StatusCode { get; set; }

        public UnsupportedException(StatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

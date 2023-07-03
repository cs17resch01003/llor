namespace LLOR.Repair.Exceptions
{
    using System;
    using LLOR.Common;
    
    public class RepairException : Exception
    {
        public StatusCode StatusCode { get; set; }

        public RepairException(StatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}

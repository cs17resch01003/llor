namespace Repair.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class ExecutionException : Exception
    {
        public List<string> ErrorData;

        public ExecutionException(List<string> errorData) : base()
        {
            ErrorData = errorData;
        }
    }
}

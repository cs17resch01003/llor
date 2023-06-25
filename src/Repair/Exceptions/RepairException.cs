namespace Repair.Exceptions
{
    using System;
    
    public class RepairException : Exception
    {
        public RepairException(string message)
            : base(message)
        {
        }
    }
}

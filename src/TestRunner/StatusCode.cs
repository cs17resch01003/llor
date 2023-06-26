namespace LLOR.TestRunner
{
    using System.Collections.Generic;
    using System.IO;
    
    public class StatusCode
    {
        public static List<StatusCode> StatusCodes = new List<StatusCode>();

        public static StatusCode Pass;

        public int Code { get; set; }

        public string Description { get; set; }

        static StatusCode()
        {
            Pass = new StatusCode(0, "pass");
            StatusCodes.Add(Pass);
        }

        private StatusCode(int statusCode, string description)
        {
            Code = statusCode;
            Description = description;
        }

        public static StatusCode GetStatusCode(int statusCode)
        {
            foreach (StatusCode status in StatusCodes)
                if (status.Code == statusCode)
                    return status;
            
            throw new InvalidDataException(statusCode.ToString());
        }

        public static StatusCode GetStatusCode(string description)
        {
            foreach (StatusCode status in StatusCodes)
                if (status.Description == description)
                    return status;
            
            throw new InvalidDataException(description);
        }
    }
}

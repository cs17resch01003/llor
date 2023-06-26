namespace LLOR.TestRunner
{
    public class ParseMessage
    {
        public string Message { get; set; }

        public int Skip { get; set; }

        public StatusCode StatusCode { get; set; }

        public ParseMessage(string message, int skip, StatusCode statusCode)
        {
            Message = message;
            Skip = skip;
            StatusCode = statusCode;
        }
    }
}

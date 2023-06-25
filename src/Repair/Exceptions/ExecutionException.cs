namespace Repair.Exceptions
{
    public class ExecutionException : Exception
    {
        public List<string> ErrorData;

        public ExecutionException(List<string> errorData) : base()
        {
            ErrorData = errorData;
        }
    }
}

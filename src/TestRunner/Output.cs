namespace LLOR.TestRunner
{
    using System.Collections.Generic;
    using System.Linq;
    using LLOR.Common;
    
    public class Output
    {
        public StatusCode StatusCode { get; set; }

        public List<string> Result { get; set; }

        public Output(StatusCode statusCode)
        {
            StatusCode = statusCode;
            Result = new List<string>();
        }

        public Output(StatusCode statusCode, List<string> result)
        {
            StatusCode = statusCode;
            Result = result;
        }

        public override string ToString()
        {
            return string.Join("\n", Result);
        }

        public override bool Equals(object? obj)
        {
            Output? output = obj as Output;
            if (output == null)
                return false;

            HashSet<string> set = new HashSet<string>(output.Result.Distinct());
            return output.StatusCode == StatusCode
                && output.Result.Count == Result.Count
                && set.SetEquals(Result.Distinct());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}

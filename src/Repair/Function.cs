namespace LLOR.Repair
{
    using LLOR.Common;
    
    public class Function
    {
        public string FunctionName { get; set; } = string.Empty;

        public Location Start { get; set; }

        public Location? End { get; set; }

        public Function(
            string functionName,
            int start,
            int? end)
        {
            FunctionName = functionName;
            Start = new Location(start, 0);
            if (end != null)
                End = new Location(end.Value, 0);
        }
    }
}

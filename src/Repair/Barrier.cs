namespace LLOR.Repair
{
    using LLOR.Common;
    
    public class Barrier
    {
        public string? Name { get; set; } = null;

        public string BarrierType { get; set; }

        public string? Function { get; set; } = null;

        public Location Location { get; set; }

        public bool Enabled { get; set; } = false;

        public Barrier(
            string barrierType,
            int line,
            int column)
        {
            BarrierType = barrierType;
            Location = new Location(line, column);
        }

        public Barrier(
            string name,
            string barrierType,
            int line,
            int column,
            string function)
        {
            Name = name;
            BarrierType = barrierType;
            Function = function;
            Location = new Location(line, column);
        }
    }
}

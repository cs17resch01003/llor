namespace LLOR.Repair
{
    using LLOR.Common;
    
    public class Barrier
    {
        public string Name { get; set; }

        public string BarrierType { get; set; }

        public string Function { get; set; }

        public string? Loop { get; set; }

        public Location Location { get; set; }

        public bool Enabled { get; set; } = false;

        public Barrier(
            string name,
            string barrierType,
            int line,
            int column,
            string function,
            string? loop)
        {
            Name = name;
            BarrierType = barrierType;
            Function = function;
            Loop = loop;
            Location = new Location(line, column);
        }
    }
}

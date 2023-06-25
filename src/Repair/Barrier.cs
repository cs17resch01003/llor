namespace Repair
{
    public class Barrier
    {
        public string Name { get; set; }

        public string BarrierType { get; set; }

        public string Function { get; set; }

        public Location Location { get; set; }

        public bool Enabled { get; set; } = false;

        public Barrier(string name, string barrierType, int line, int column, string function)
        {
            Name = name;
            BarrierType = barrierType;
            Function = function;
            Location = new Location(line, column);
        }
    }
}

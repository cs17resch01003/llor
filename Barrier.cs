namespace LLOR
{
    public class Barrier
    {
        public string Name { get; set; }

        public string BarrierType { get; set; }

        public Location Location { get; set; }

        public bool Enabled { get; set; } = false;

        public Barrier(string name, string barrierType, int line, int column)
        {
            Name = name;
            BarrierType = barrierType;
            Location = new Location(line, column);
        }
    }
}

namespace LLOR
{
    public class Barrier
    {
        public string Name { get; set; }

        public Location Location { get; set; }

        public bool Enabled { get; set; } = false;

        public Barrier(string name, int line, int column)
        {
            Name = name;
            Location = new Location(line, column);
        }
    }
}

namespace LLOR
{
    public class ExistingBarrier
    {
        public string BarrierType { get; set; }

        public Location Location { get; set; }

        public ExistingBarrier(string barrierType, int line, int column)
        {
            BarrierType = barrierType;
            Location = new Location(line, column);
        }
    }
}

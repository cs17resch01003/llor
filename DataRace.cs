namespace LLOR
{
    public class DataRace
    {
        public Location? Source { get; set; }

        public Location? Sink { get; set; }

        public List<Barrier> Barriers { get; set; } = new List<Barrier>();

        public void PopulateBarriers(IEnumerable<Barrier> barriers)
        {
            if (Source == null || Sink == null)
                throw new Exception("Invalid data race!");

            foreach(Barrier barrier in barriers)
                if (barrier.Location.IsBetween(Source, Sink))
                    Barriers.Add(barrier);
        }
    }
}

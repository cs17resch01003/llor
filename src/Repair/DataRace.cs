namespace Repair
{
    using System.Collections.Generic;
    using System.Linq;

    public class DataRace
    {
        public Location? Source { get; set; }

        public Location? Sink { get; set; }

        public List<Barrier> Barriers { get; set; } = new List<Barrier>();

        public void PopulateBarriers(IEnumerable<Barrier> barriers)
        {
            if (Source != null && Sink != null)
            {
                foreach(Barrier barrier in barriers)
                    if (barrier.Location.IsBetween(Source, Sink))
                        Barriers.Add(barrier);
            }

            if (!Barriers.Any())
            {
                foreach(Barrier barrier in barriers)
                    Barriers.Add(barrier);
            }
        }
    }
}

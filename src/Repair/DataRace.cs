namespace LLOR.Repair
{
    using System.Collections.Generic;
    using System.Linq;

    public class DataRace : LLOR.Common.DataRace
    {
        public List<Barrier> Barriers { get; set; } = new List<Barrier>();

        public void PopulateBarriers(IEnumerable<Barrier> barriers)
        {
            if (Source != null && Sink != null)
            {
                foreach(Barrier barrier in barriers.Where(x => !x.Enabled))
                    if (barrier.Location.IsBetween(Source, Sink))
                        Barriers.Add(barrier);
            }

            if (!Barriers.Any())
            {
                foreach(Barrier barrier in barriers.Where(x => !x.Enabled))
                    Barriers.Add(barrier);
            }
        }
    }
}

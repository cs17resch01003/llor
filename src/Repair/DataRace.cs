namespace LLOR.Repair
{
    using System.Collections.Generic;
    using System.Linq;

    public class DataRace : Common.DataRace
    {
        public int? Start { get; set; } = null;

        public int? End { get; set; } = null;

        public List<Barrier> Barriers { get; set; } = new List<Barrier>();

        public void PopulateMetadata(IEnumerable<Barrier> barriers)
        {
            if (Source != null && Sink != null)
            {
                if (Source.Line > Sink.Line)
                {
                    Start = Sink.Line;
                    End = Source.Line;
                }
                else
                {
                    Start = Source.Line;
                    End = Sink.Line;
                }

                foreach(Barrier barrier in barriers.Where(x => !x.Enabled))
                    if (barrier.Location.IsBetween(Source, Sink))
                        Barriers.Add(barrier);
            }

            if (!Barriers.Any())
            {
                foreach(Barrier barrier in barriers.Where(x => !x.Enabled))
                    Barriers.Add(barrier);
            }

            Barriers = Barriers.Any(x => x.BarrierType == "ordered") ?
                Barriers.Where(x => x.BarrierType == "ordered").ToList() : Barriers;
        }
    }
}

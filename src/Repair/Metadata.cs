namespace LLOR.Repair
{
    using System.Collections.Generic;
    
    public class Metadata
    {
        public Dictionary<string, Barrier> Barriers { get; set; } = new Dictionary<string, Barrier>();

        public List<Barrier> Existing { get; set; } = new List<Barrier>();

        public List<Function> Functions { get; set; } = new List<Function>();
    }
}

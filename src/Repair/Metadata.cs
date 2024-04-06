namespace LLOR.Repair
{
    using System.Collections.Generic;
    
    public class Metadata
    {
        public Dictionary<string, Barrier> Barriers { get; set; } = new Dictionary<string, Barrier>();

        public List<ExistingBarrier> Existing { get; set; } = new List<ExistingBarrier>();
    }
}

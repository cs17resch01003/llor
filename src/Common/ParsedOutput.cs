namespace LLOR.Common
{
    using System.Collections.Generic;

    public class ParsedOutput
    {
        public StatusCode StatusCode { get; set; } = StatusCode.Pass;

        public List<DataRace> Races { get; set; } = new List<DataRace>();

        public List<string> Unparsed { get; set; } = new List<string>();
    }
}

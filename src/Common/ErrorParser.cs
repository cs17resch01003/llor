namespace LLOR.Common
{
    using System.Collections.Generic;
    using System.Linq;
    
    public static class ErrorParser
    {
        public static ParsedOutput ParseErrorOutput(List<string> output)
        {
            ParsedOutput parsed = new ParsedOutput();
            List<ParseMessage> messages = new List<ParseMessage>
            {
                new ParseMessage("Region is Data Race Free.", 2, StatusCode.Pass),
                new ParseMessage("Tasking and DOACROSS loops are not supported.", 1, StatusCode.Unsupported),
                new ParseMessage("Directive Not Analyzed by the verifier.", 1, StatusCode.Unsupported),
                new ParseMessage("/usr/bin/ld", 0, StatusCode.Fail),
                new ParseMessage("undefined reference", 0, StatusCode.Fail),
            };

            DataRace? current = null;
            for(int i = 0; i < output.Count; i++)
            {
                string line = output.ElementAt(i);
                if (line == "Data Race detected.")
                {
                    current = new DataRace();
                    parsed.Races.Add(current);
                }
                else if (line.StartsWith("Source") || line.StartsWith("Sink"))
                {
                    if (current == null)
                    {
                        parsed.Unparsed.Add(line);
                        break;
                    }

                    string[] parts = line.Split(":");
                    if (line.StartsWith("Source"))
                        current.Source = new Location(int.Parse(parts[2]), int.Parse(parts[3]));
                    else
                        current.Sink = new Location(int.Parse(parts[2]), int.Parse(parts[3]));
                }
                else
                {
                    ParseMessage? message =
                        messages.FirstOrDefault(x => line.Contains(x.Message));
                    if (message != null)
                    {
                        i += message.Skip;
                        if (parsed.StatusCode == StatusCode.Pass)
                            parsed.StatusCode = message.StatusCode;
                    }
                    else
                        parsed.Unparsed.Add(line);
                }
            }

            return parsed;
        }
    }
}

namespace LLOR
{
    public class Location
    {
        public int Line { get; set; }

        public int Column { get; set; }

        public Location(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public bool IsBetween(Location first, Location second)
        {
            if (Line == first.Line && Line == second.Line)
                // LLVM evaluates the right-side operation before the left-side
                return first.Column >= Column && Column >= second.Column;
            return second.Line >= Line && Line > first.Line;
        }
    }
}

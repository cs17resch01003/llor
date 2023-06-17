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
                // LLOV gives the column number of the assignment operator
                return first.Column >= Column && second.Column >= Column;
            return second.Line >= Line && Line > first.Line;
        }
    }
}

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

        public override string ToString()
        {
            return $"{Line},{Column}";
        }

        public override bool Equals(object? obj)
        {
            Location? location = obj as Location;
            if (location == null)
                return false;

            return location.Line == Line && location.Column == Column;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}

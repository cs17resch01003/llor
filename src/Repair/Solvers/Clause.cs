namespace Repair.Solvers
{
    public class Clause
    {
        public List<Literal> Literals { get; set; }

        public Clause()
        {
            Literals = new List<Literal>();
        }

        public void Add(Literal literal)
        {
            Literals.Add(literal);
        }

        public void AddRange(IEnumerable<Literal> literals)
        {
            Literals.AddRange(literals);
        }

        public override string ToString()
        {
            return string.Join(" ", Literals.Distinct().OrderBy(x => x.Value)
                .ThenBy(x => int.Parse(x.Variable.Replace("b", string.Empty))).Select(x => x.ToString()));
        }

        public override bool Equals(object? obj)
        {
            Clause? clause = obj as Clause;
            if (clause == null)
                return false;

            return clause.ToString().Equals(ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}

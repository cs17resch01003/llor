namespace LLOR.Repair.Solvers
{
    public class Literal
    {
        public string Variable { get; set; }

        public int Weight { get; set; }

        public bool Value { get; set; }

        public Literal(string variable, bool value)
        {
            Variable = variable;
            Value = value;
        }

        public Literal(string variable, int weight, bool value)
        {
            Variable = variable;
            Weight = weight;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", Value ? string.Empty : "-", Variable);
        }

        public override bool Equals(object? obj)
        {
            Literal? literal = obj as Literal;
            if (literal == null)
                return false;

            return literal.Variable == Variable && literal.Value == Value;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}

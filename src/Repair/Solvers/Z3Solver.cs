namespace Repair.Solvers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Z3;

    public abstract class Z3Solver
    {
        protected Dictionary<string, Z3Variable> GetVariables(Context context, IEnumerable<Clause> input)
        {
            Dictionary<string, Z3Variable> variables = new Dictionary<string, Z3Variable>();

            IEnumerable<string> variableNames = input.SelectMany(x => x.Literals).Select(x => x.Variable).Distinct();
            foreach (string variableName in variableNames)
                variables.Add(variableName, new Z3Variable(context, variableName));

            return variables;
        }

        protected List<BoolExpr> GenerateClauses(Context context, IEnumerable<Clause> input,
            Dictionary<string, Z3Variable> variables)
        {
            List<BoolExpr> clauses = new List<BoolExpr>();
            foreach (Clause clause in input)
                clauses.Add(GenerateClause(context, clause, variables));

            return clauses;
        }

        private BoolExpr GenerateClause(Context context, Clause clause, Dictionary<string, Z3Variable> variables)
        {
            List<BoolExpr> literals = new List<BoolExpr>();
            foreach (Literal literal in clause.Literals)
            {
                Z3Variable variable = variables[literal.Variable];
                literals.Add(literal.Value ? variable.Positive : variable.Negative);
            }

            return context.MkOr(literals);
        }

        protected class Z3Variable
        {
            public string VariableName;

            public BoolExpr Positive;

            public BoolExpr Negative;

            public Z3Variable(Context context, string variableName)
            {
                VariableName = variableName;
                Positive = context.MkBoolConst(variableName);
                Negative = context.MkNot(Positive);
            }
        }
    }
}

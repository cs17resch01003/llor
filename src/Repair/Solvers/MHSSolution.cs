namespace LLOR.Repair.Solvers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Z3;

    internal class MHSSolution
    {
        private readonly Dictionary<Clause, State> clauseLookup;

        internal readonly Dictionary<string, List<Clause>> VariableLookup;

        internal readonly Dictionary<string, bool> Assignments;

        public MHSSolution(IEnumerable<Clause> clauses)
        {
            clauseLookup = new Dictionary<Clause, State>();

            VariableLookup = new Dictionary<string, List<Clause>>();
            Assignments = new Dictionary<string, bool>();

            foreach (Clause clause in clauses)
            {
                State state = new State(clause);
                foreach (string variable in state.VariableLookup.Keys)
                {
                    if (!VariableLookup.ContainsKey(variable))
                        VariableLookup.Add(variable, new List<Clause>());
                    VariableLookup[variable].Add(clause);
                }

                clauseLookup.Add(clause, state);
            }
        }

        public void SetAssignment(string variable, bool value)
        {
            Assignments.Add(variable, value);
            foreach (Clause clause in VariableLookup[variable])
            {
                State state = clauseLookup[clause];
                state.UnassignedLiterals--;

                if (value)
                    state.Sat = Status.SATISFIABLE;
            }
        }

        public List<Clause> GetActiveUnitClauses()
        {
            return clauseLookup.Where(x => x.Value.Sat == Status.UNKNOWN && x.Value.UnassignedLiterals == 1)
                .Select(x => x.Key).ToList();
        }

        public List<Clause> GetActiveClauses()
        {
            return clauseLookup.Where(x => x.Value.Sat == Status.UNKNOWN)
                .Select(x => x.Key).ToList();
        }

        public bool IsSolutionFound()
        {
            return !clauseLookup.Any(x => x.Value.Sat == Status.UNKNOWN);
        }

        private class State
        {
            internal readonly Dictionary<string, Literal> VariableLookup;

            internal Status Sat { get; set; } = Status.UNKNOWN;

            internal ushort UnassignedLiterals { get; set; }

            internal State(Clause clause)
            {
                VariableLookup = new Dictionary<string, Literal>();
                foreach (Literal literal in clause.Literals)
                {
                    VariableLookup.Add(literal.Variable, literal);
                    UnassignedLiterals++;
                }
            }
        }
    }
}

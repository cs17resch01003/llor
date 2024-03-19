namespace LLOR.Repair.Solvers
{
    using System.Collections.Generic;
    using System.Linq;

    public class MHSSolver
    {
        private IEnumerable<Clause> clauses { get; set; }

        public MHSSolver(IEnumerable<Clause> clauses)
        {
            this.clauses = clauses;
        }

        public Dictionary<string, bool> Solve(out SolverStatus status)
        {
            MHSSolution solution = new MHSSolution(clauses);
            Solve(solution);

            status = SolverStatus.Satisfiable;
            return solution.Assignments;
        }

        private void Solve(MHSSolution solution)
        {
            // unit literal propogation
            List<Clause> unit_clauses = solution.GetActiveUnitClauses();
            while (unit_clauses.Any())
            {
                UnitLiteralPropogation(solution, unit_clauses);
                unit_clauses = solution.GetActiveUnitClauses();
            }

            // find MHS for all active clauses
            List<Clause> clauses = solution.GetActiveClauses();
            while (clauses.Any())
            {
                ApplyMHS(solution, clauses);
                clauses = solution.GetActiveClauses();
            }

            // set everything else to false
            foreach (string variable in solution.VariableLookup.Keys)
                if (!solution.Assignments.ContainsKey(variable))
                    solution.SetAssignment(variable, false);
        }

        private void UnitLiteralPropogation(MHSSolution solution, List<Clause> clauses)
        {
            foreach (Clause clause in clauses)
                foreach (Literal literal in clause.Literals)
                    if (!solution.Assignments.ContainsKey(literal.Variable))
                    {
                        solution.SetAssignment(literal.Variable, true);
                        return;
                    }
        }

        private void ApplyMHS(MHSSolution solution, List<Clause> clauses)
        {
            IEnumerable<string> variables = clauses.SelectMany(x => x.Literals)
                .Select(x => x.Variable).Distinct();

            string chosen = string.Empty;
            double max_total_weight = 0;

            foreach (string variable in variables)
            {
                if (!solution.Assignments.ContainsKey(variable))
                {
                    int unknown_clauses = clauses.Count(x => x.Literals.Select(y => y.Variable).Contains(variable));
                    double total_weight = unknown_clauses;

                    if (total_weight > max_total_weight)
                    {
                        max_total_weight = total_weight;
                        chosen = variable;
                    }
                }
            }

            solution.SetAssignment(chosen, true);
        }
    }
}

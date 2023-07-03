namespace LLOR.Repair
{
    using System.Collections.Generic;
    using System.Linq;
    using LLOR.Common;
    using LLOR.Repair.Exceptions;
    using LLOR.Repair.Solvers;

    public class Solver
    {
        public Dictionary<string, bool> Solve(IEnumerable<DataRace> races)
        {
            IEnumerable<Clause> clauses = GenerateClauses(races);
            return Solve(clauses);
        }

        private IEnumerable<Clause> GenerateClauses(IEnumerable<DataRace> races)
        {
            List<Clause> clauses = new List<Clause>();
            foreach (DataRace race in races)
            {
                Clause clause = new Clause();
                foreach (string variable in race.Barriers.Select(x => x.Name))
                    clause.Add(new Literal(variable, true));

                clauses.Add(clause);
            }

            return clauses.Distinct().ToList();
        }

        private Dictionary<string, bool> Solve(IEnumerable<Clause> clauses)
        {
            Dictionary<string, bool>? solution;
            SolverStatus status;

            List<string> variables = clauses.SelectMany(x => x.Literals)
                .Select(x => x.Variable).Distinct().ToList();
            IEnumerable<Clause> soft_clauses = GenerateSoftClauses(variables);

            MaxSATSolver solver = new MaxSATSolver(clauses, soft_clauses);
            solution = solver.Solve(out status);

            if (status == SolverStatus.Unsatisfiable || solution == null)
                throw new RepairException(
                    StatusCode.RepairError,
                    "The program could not be repaired because of unsatisfiable clauses!");

            return solution;
        }

        private IEnumerable<Clause> GenerateSoftClauses(List<string> variables)
        {
            List<Clause> clauses = new List<Clause>();
            foreach (string variable in variables)
            {
                Clause clause = new Clause();
                clause.Add(new Literal(variable, false));

                clauses.Add(clause);
            }

            return clauses;
        }
    }
}

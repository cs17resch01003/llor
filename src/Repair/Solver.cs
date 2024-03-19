namespace LLOR.Repair
{
    using System.Collections.Generic;
    using System.Linq;
    using LLOR.Common;
    using LLOR.Repair.Diagnostics;
    using LLOR.Repair.Exceptions;
    using LLOR.Repair.Solvers;

    public class Solver
    {
        public Dictionary<string, bool> Solve(IEnumerable<DataRace> races, SolverType type)
        {
            IEnumerable<Clause> clauses = GenerateClauses(races);
            return Solve(clauses, type);
        }

        public Dictionary<string, bool> Optimize(IEnumerable<DataRace> races, Dictionary<string, bool> solution)
        {
            IEnumerable<Clause> clauses = GenerateClauses(races);
            while (true)
            {
                int total_weight = solution.Where(x => x.Value == true).Count();
                using (Watch watch = new Watch(Measure.Optimization))
                {
                    SolverStatus status;

                    Optimizer optimizer = new Optimizer(clauses);
                    Dictionary<string, bool>? assignments = optimizer.Solve(--total_weight, out status);

                    if (status == SolverStatus.Unsatisfiable)
                        return solution;
                    else if (assignments != null)
                        solution = assignments;
                }
            }
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

        private Dictionary<string, bool> Solve(IEnumerable<Clause> clauses, SolverType type)
        {
            Dictionary<string, bool>? solution;
            SolverStatus status;

            if (type == SolverType.mhs)
                solution = SolveMHS(clauses, out status);
            else if (type == SolverType.MaxSAT)
                solution = SolveMaxSAT(clauses, out status);
            else
                throw new RepairException(StatusCode.RepairError, "Invalid solver type!");

            if (status == SolverStatus.Unsatisfiable || solution == null)
                throw new RepairException(
                    StatusCode.RepairError,
                    "The program could not be repaired because of unsatisfiable clauses!");

            return solution;
        }

        private Dictionary<string, bool>? SolveMHS(IEnumerable<Clause> clauses, out SolverStatus status)
        {
            using (Watch watch = new Watch(Measure.mhs))
            {
                MHSSolver solver = new MHSSolver(clauses);
                return solver.Solve(out status);
            }
        }

        private Dictionary<string, bool>? SolveMaxSAT(IEnumerable<Clause> clauses, out SolverStatus status)
        {
            using (Watch watch = new Watch(Measure.MaxSAT))
            {
                List<string> variables = clauses.SelectMany(x => x.Literals)
                    .Select(x => x.Variable).Distinct().ToList();
                IEnumerable<Clause> soft_clauses = GenerateSoftClauses(variables);

                MaxSATSolver solver = new MaxSATSolver(clauses, soft_clauses);
                return solver.Solve(out status);
            }
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

        public enum SolverType
        {
            mhs,
            MaxSAT,
            Optimizer,
        }
    }
}

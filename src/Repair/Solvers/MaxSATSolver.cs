namespace Repair.Solvers
{
    using Microsoft.Z3;

    public class MaxSATSolver : Z3Solver
    {
        private IEnumerable<Clause> hard_clauses { get; set; }

        private IEnumerable<Clause> soft_clauses { get; set; }

        public MaxSATSolver(IEnumerable<Clause> hard_clauses, IEnumerable<Clause> soft_clauses)
        {
            this.hard_clauses = hard_clauses;
            this.soft_clauses = soft_clauses;
        }

        public Dictionary<string, bool>? Solve(out SolverStatus status)
        {
            // Use Z3 and figure out the variable assignments
            using (Context context = new Context())
            {
                Dictionary<string, Z3Variable> variables = GetVariables(context, this.hard_clauses.Union(this.soft_clauses));

                List<BoolExpr> hard_clauses = GenerateClauses(context, this.hard_clauses, variables);
                List<BoolExpr> soft_clauses = GenerateClauses(context, this.soft_clauses, variables);

                Optimize optimize = context.MkOptimize();
                optimize.Assert(hard_clauses);
                foreach (BoolExpr clause in soft_clauses)
                    optimize.AssertSoft(clause, 1, "group");

                Status solver_status = optimize.Check();
                if (solver_status == Status.SATISFIABLE)
                {
                    Dictionary<string, bool> assignments = new Dictionary<string, bool>();
                    foreach (string variable in variables.Keys)
                    {
                        Expr expr = optimize.Model.Evaluate(variables[variable].Positive);
                        if (expr.BoolValue != Z3_lbool.Z3_L_UNDEF)
                            assignments.Add(variable, expr.BoolValue == Z3_lbool.Z3_L_TRUE ? true : false);
                    }

                    status = SolverStatus.Satisfiable;
                    return assignments;
                }

                status = SolverStatus.Unsatisfiable;
                return null;
            }
        }
    }
}

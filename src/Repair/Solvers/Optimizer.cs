namespace LLOR.Repair.Solvers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Z3;

    public class Optimizer : Z3Solver
    {
        private IEnumerable<Clause> clauses { get; set; }

        public Optimizer(IEnumerable<Clause> clauses)
        {
            this.clauses = clauses;
        }

        public Dictionary<string, bool>? Solve(int limit, out SolverStatus status)
        {
            // Use Z3 and figure out the variable assignments
            using (Context context = new Context())
            {
                Dictionary<string, Z3Variable> variables = GetVariables(context, this.clauses);
                List<BoolExpr> clauses = GenerateClauses(context, this.clauses, variables);

                Solver solver = context.MkSolver();
                solver.Assert(clauses.ToArray());

                Params parameters = context.MkParams();
                parameters.Add("timeout", 10000);
                solver.Parameters = parameters;

                BoolExpr condition = context.MkPBLe(
                    variables.Values.Select(x => 1).ToArray(),
                    variables.Values.Select(x => x.Positive).ToArray(),
                    limit);
                solver.Assert(condition);

                Status solver_status = solver.Check();
                if (solver_status == Status.SATISFIABLE)
                {
                    Dictionary<string, bool> assignments = new Dictionary<string, bool>();
                    foreach (string variable in variables.Keys)
                    {
                        Expr expr = solver.Model.Evaluate(variables[variable].Positive);
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

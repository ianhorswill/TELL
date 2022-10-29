using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TELL
{
    /// <summary>
    /// A Goal represents a predicate applied to arguments, e.g. p["a"], p[variable], etc.
    /// Goals are used as the arguments to Prover.Prove, but also as the Head and Body of Rules.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebugName) + "}")]
    public abstract class AnyGoal
    {
        /// <summary>
        /// Predicate being called
        /// </summary>
        public readonly AnyPredicate Predicate;

        public readonly AnyTerm[] Arguments;
        
        /// <summary>
        /// Make a new goal object
        /// </summary>
        protected AnyGoal(AnyPredicate predicate, AnyTerm[] args)
        {
            Predicate = predicate;
            Arguments = args;
        }

        /// <summary>
        /// Add this goal as a rule in the predicate's list of rules, but without any subgoals.
        /// The Head of the rule will be this Goal, and the Body will be empty.
        /// </summary>
        public void Fact() => Predicate.AddRule(new Rule(this));

        /// <summary>
        /// Add this goal as a rule in the predicate's list of rules, with the specified subgoals in its body.
        /// So the Head of the rule will be this Goal and the Body will be the subgoals
        /// </summary>
        /// <param name="subgoals">Subgoals to include in the goal's body.</param>
        public void If(params AnyGoal[] subgoals) => Predicate.AddRule(new Rule(this, subgoals));

        /// <summary>
        /// Make a copy of this rule, replacing any arguments that appear in the Dictionary with their values in the hash table.
        /// This is only called from Rule.Copy().
        /// </summary>
        /// <param name="vars">Mapping used to selectively rewrite arguments</param>
        /// <returns></returns>
        internal InstantiatedGoal Instantiate(Dictionary<AnyTerm,AnyTerm>? vars) =>
            new InstantiatedGoal(Predicate, Arguments.Select(t => t.Instantiate(vars)).ToArray());

        #region Printing
        /// <summary>
        /// Convert the goal to a human-readable string, for purposes of printing.
        /// </summary>
        public override string ToString()
        {
            var b = new StringBuilder();
            ToString(b);
            return b.ToString();
        }
        
        /// <summary>
        /// Add the printed representation of the goal to this StringBuilder.
        /// </summary>
        public void ToString(StringBuilder b)
        {
            b.Append(Predicate.Name);
            b.Append('[');
            var first = true;
            foreach (var arg in Arguments)
            {
                if (first)
                    first = false;
                else
                    b.Append(", ");

                b.Append(arg);
            }

            b.Append(']');
        }

        /// <summary>
        /// This is just so that this appears in human-readable form in the debugger
        /// </summary>
        public string DebugName => ToString();

        public bool IsTrue => Prover.CanProve(this);

        public bool IsFalse => Prover.CanProve(this);

        public T SolveFor<T>(Var<T> v) => Prover.SolveFor(v, this);
        public List<T> SolveForAll<T>(Var<T> v) => Prover.SolveForAll(v, this);

        #endregion
    }
}

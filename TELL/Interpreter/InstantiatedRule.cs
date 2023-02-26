using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TELL.Interpreter
{
    /// <summary>
    /// A copy of a Rule made for a specific call to that rule.
    /// Copying the rule prevents multiple calls to the rule from sharing the same variables.
    /// InstantiatedRules have InstantiatedGoals rather than normal Goals.  And InstantiatedGoals
    /// are untyped and don't wrap constants in Constant objects
    /// </summary>
    [DebuggerDisplay("{DebugName}")]
    internal readonly struct InstantiatedRule
    {
        /// <summary>
        /// Goal this rule is used to prove
        /// </summary>
        public readonly InstantiatedGoal Head;
        /// <summary>
        /// Subgoals that must be true to prove the Head
        /// </summary>
        public readonly InstantiatedGoal[] Body;

        public InstantiatedRule(InstantiatedGoal head, InstantiatedGoal[] subgoals)
        {
            Head = head;
            Body = subgoals;
        }

        public override string ToString()
        {
            var b = new StringBuilder();
            b.Append(Head);
            if (Body.Length == 0)
                b.Append(".Fact");
            else
            {
                b.Append(".If(");
                b.Append(string.Join(", ", Body.Select(g => g.ToString())));
            }

            return b.ToString();
        }

        public string DebugName => ToString();
    }
}

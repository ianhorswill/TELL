using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TELL
{
    /// <summary>
    /// An if/then rule that can be used to prove a predicate.
    /// 
    /// It states that if all the subgoals in the Body are true, then the goal in the Head must be true.
    /// Therefore, if you you want to prove the goal in the Head, try to prove the subgoals in the body.
    /// The Body can be empty, in which case this rule says the Head is always true (is a "fact").
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebugName) + "}")]
    public class Rule
    {
        /// <summary>
        /// Goal that this rule can be used to prove
        /// </summary>
        public readonly AnyGoal Head;
        /// <summary>
        /// Subgoals that you have to prove in order to prove the Head
        /// </summary>
        public readonly AnyGoal[] Body;
        
        /// <summary>
        /// Saved list of all the variable names appearing in the Head and Body
        /// These are needed because every time we try this rule, we need to make a copy
        /// of it with fresh Variable objects.  Having this list makes the copying process easier
        /// </summary>
        public readonly HashSet<AnyTerm> Variables = new HashSet<AnyTerm>();

        /// <summary>
        /// Make a new rule for proving a goal
        /// </summary>
        /// <param name="head">Goal this can prove</param>
        /// <param name="body">Subgoals needed to prove the goal</param>
        public Rule(AnyGoal head, params AnyGoal[] body)
        {
            Head = head;
            Body = body;

            void AddVars(AnyGoal g)
            {
                foreach (var s in g.Arguments.Where(t=>t.IsVariable))
                    Variables.Add(s);
            }

            AddVars(Head);
            foreach (var sub in Body)
                AddVars(sub);
        }

        /// <summary>
        /// Make a copy of the rule, replacing all the strings that look like variable names (e.g. "?x")
        /// with actual variables.  The copies are InstantiatedRules rather than Rules and they contain
        /// InstantiatedGoals.  This removes the Term wrappers around constants.
        ///
        /// We need to use a new copy of the rule every time we try to use it to prove something,
        /// otherwise things break when we have rules that are used more than once in a proof
        /// (e.g. if there's recursion).
        /// </summary>
        internal InstantiatedRule Instantiate()
        {
            var variables = new Dictionary<AnyTerm,AnyTerm>();
            foreach (var v in Variables)
                variables[v] = v.Clone();

            var newBody = new InstantiatedGoal[Body.Length];

            for (var i = 0; i < Body.Length; i++)
                newBody[i] = Body[i].Instantiate(variables);

            return new InstantiatedRule(Head.Instantiate(variables), newBody);
        }

        #region Printing
        /// <summary>
        /// This is just here so the Debugger knows how to display the rule in human-readable format.
        /// </summary>
        public string DebugName => ToString();
        
        /// <summary>
        /// Convert the rule to a human-readable string
        /// </summary>
        public override string ToString()
        {
            var b = new StringBuilder();
            ToString(b);
            return b.ToString();
        }
        
        /// <summary>
        /// Print the rule in human-readable format to the StringBuilder
        /// </summary>
        public void ToString(StringBuilder b)
        {
            Head.ToString(b);
            if (Body.Length > 0)
            {
                b.Append(" if ");
                var first = true;
                foreach (var subgoal in Body)
                {
                    if (first)
                        first = false;
                    else
                        b.Append(", ");

                    b.Append(subgoal);
                }
            }
            b.Append('.');
        }
        #endregion
    }
}

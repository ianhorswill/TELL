using System;
using System.Collections.Generic;
using System.Linq;

namespace TELL.Interpreter
{
    /// <summary>
    /// Implements the algorithm for proving Goals using Rules
    /// </summary>
    public static class Prover
    {
        /// <summary>
        /// Type of methods used to try to prove instantiates of a particular predicate
        /// Or, more properly, instantiates of goals whose predicate is the predicate in question.
        ///
        /// For Rule-based predicates, the implementation will be ProveUsingRules.  For primitives
        /// it will be custom code or code generated using a wrapper from Primitives.
        /// </summary>
        /// <param name="g">Goal to prove</param>
        /// <param name="s">Substitutions currently in effect</param>
        /// <param name="k">Continuation to call if we're successful</param>
        /// <returns>False if we fail, otherwise whatever k returns.</returns>
        public delegate bool PredicateImplementation(InstantiatedGoal g, Substitution? s, SuccessContinuation k);

        /// <summary>
        /// A continuation is a method to call when a goal or subgoal is successfully proven.
        /// Since proving can fill in values for variables, the prover will pass the continuation the
        /// final Substitution used in the proof.
        /// </summary>
        /// <param name="newSubstitution"></param>
        /// <returns>True if the continuation accepted the final Substitution.  If it's false, then the prover should try to backtrack.</returns>
        public delegate bool SuccessContinuation(Substitution? newSubstitution);

        /// <summary>
        /// Try to prove goal given the specified substitution
        /// </summary>
        /// <param name="g">Goal to prove</param>
        /// <param name="s">Variable substitutions in effect</param>
        /// <param name="k">Success continuation to call with final substitutions</param>
        /// <returns>True if successful and continuation returned true</returns>
        internal static bool ProveUsingRules(InstantiatedGoal g, Substitution? s, SuccessContinuation k)
        // G is true if it can be proven by any rule
        // Don't forget to copy the rule before trying to prove it
            => g.Predicate.Rules.Any(r => ProveUsingRule(g, r.Instantiate(), s, k));

        /// <summary>
        /// Try to prove goal using the specified rule
        /// This will work if the goal can be unified with the head of the rule and all the subgoals can
        /// also be proven.
        /// </summary>
        /// <param name="g">Goal to prove</param>
        /// <param name="r">Rule to try to use to prove it.</param>
        /// <param name="s">Substitutions in effect</param>
        /// <param name="k">Success continuation to call with final substitutions</param>
        /// <returns>Successful and continuation returned true</returns>
        internal static bool ProveUsingRule(InstantiatedGoal g, InstantiatedRule r, Substitution? s, SuccessContinuation k)
            => Unifier.UnifyArrays(g.Arguments, r.Head.Arguments, s, out var n)
               && ProveSubgoals(r, 0, n, k);

        /// <summary>
        /// Try to prove all the subgoals of the body of rule, starting at position index
        /// </summary>
        /// <param name="rule">Rule whose subgoals to prove</param>
        /// <param name="index">position of the next subgoal in the body</param>
        /// <param name="s">Substitutions in effect</param>
        /// <param name="k">Success continuation</param>
        /// <returns>True if everything worked and continuation returned true</returns>
        private static bool ProveSubgoals(InstantiatedRule rule, int index, Substitution? s, SuccessContinuation k)
            => index < rule.Body.Length
               ? rule.Body[index].Prove(s, n => ProveSubgoals(rule, index + 1, n, k))
               : k(s);

        #region Entry points
        /// <summary>
        /// Try to prove goal.  Return true if successful, otherwise false
        /// </summary>
        /// <param name="g">goal to prove</param>
        /// <returns>True if it was successful</returns>
        public static bool CanProve(Goal g) => g.Instantiate(null).Prove(null, _ => true);

        internal static T Solve<T>(Goal g, Func<Substitution?, T> outputFunc)
        {
            T result = default;
            if (!g.Instantiate(null).Prove(null,
                    b =>
                    {
                        result = outputFunc(b);
                        return true;
                    }))
                throw new Exception($"Can't prove goal {g}");
            return result!;
        }

        internal static IEnumerable<T> SolveAll<T>(Goal g, Func<Substitution?, T> outputFunc)
        {
            var result = new List<T>();
            g.Instantiate(null).Prove(null,
                b =>
                {
                    result.Add(outputFunc(b));
                    return false;
                });
            return result;
        }

        /// <summary>
        /// Try to prove goal.  If successful, return the final value of the variable.  If not successful, throw an exception.
        /// </summary>
        /// <param name="v">Variable to find the value of</param>
        /// <param name="g">Goal to try to prove; it should include the variable as one of its arguments.</param>
        /// <returns>Final value of the variable</returns>
        public static T SolveFor<T>(Var<T> v, Goal g)
        {
            T result = default;
            if (!g.Instantiate(null).Prove(null,
                b =>
                {
                    // Remember solution
                    result = Unifier.DereferenceToConstant<T>(v, b)!;
                    return true;
                }))
                throw new Exception($"Can't prove goal {g}");
            return result!;
        }

        /// <summary>
        /// Find *all* the solutions to the goal and return the list of values of the variable in each one.
        /// </summary>
        /// <param name="v">Variable to find the value of</param>
        /// <param name="g">Goal to prove.  It should include the variable as an argument</param>
        /// <returns>List of all values of the variable for all solutions.  If there are no solutions, the list will be empty.</returns>
        public static List<T> SolveForAll<T>(Var<T> v, Goal g)
        {
            var result = new List<T>();
            g.Instantiate(null).Prove(null,
                b =>
                {
                    // Remember this solution
                    result.Add(Unifier.DereferenceToConstant<T>(v, b)!);
                    // Force backtracking
                    return false;
                });
            return result;
        }
        #endregion
    }
}

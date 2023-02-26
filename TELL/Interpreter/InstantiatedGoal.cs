using System.Diagnostics;
using System.Linq;

namespace TELL.Interpreter
{
    /// <summary>
    /// These are copies of Goals that have been filled in with fresh variables and the raw
    /// C# objects for constants.
    /// </summary>
    [DebuggerDisplay("{DebugName}")]
    public readonly struct InstantiatedGoal
    {
        /// <summary>
        /// Predicate to be called
        /// </summary>
        public readonly Predicate Predicate;
        /// <summary>
        /// Arguments to the predicate; these are real values, not Term objects
        /// </summary>
        public readonly object?[] Arguments;

        internal InstantiatedGoal(Predicate predicate, object?[] arguments)
        {
            Predicate = predicate;
            Arguments = arguments;
        }

        /// <summary>
        /// Try to prove this goal using the specified substitution
        /// If successful, call the continuation with the resulting substitution and return
        /// its result.  Continuations can be called multiple times for multiple successes.
        /// </summary>
        /// <param name="s">Variable substitution to use</param>
        /// <param name="k">Continuation to call on success</param>
        /// <returns>True if both this rule and the continuation are successful.</returns>
        public bool Prove(Substitution? s, Prover.SuccessContinuation k) => Predicate.Implementation(this, s, k);

        /// <inheritdoc />
        public override string ToString() =>
            $"{Predicate.Name}[{string.Join(", ", Arguments.Select(a => a == null ? "null" : a.ToString()))}]";

        private string DebugName => ToString();
    }
}

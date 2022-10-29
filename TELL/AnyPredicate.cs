using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TELL
{
    /// <summary>
    /// Represents a predicate you can call as a goal.
    /// A predicate has a name, and a set of rules for when it can be true.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public abstract class AnyPredicate
    {
        /// <summary>
        /// Human-readable name for this predicate
        /// </summary>
        public readonly string Name;
        public readonly Prover.PredicateImplementation Implementation;
        public readonly bool IsPrimitive;

        /// <summary>
        /// Make a new predicate
        /// </summary>
        protected AnyPredicate(string name, Prover.PredicateImplementation implementation)
        {
            Name = name;
            Implementation = implementation;
            IsPrimitive = implementation != Prover.ProveUsingRules;
        }

        protected AnyPredicate(string name) : this(name, Prover.ProveUsingRules)
        {
        }

        /// <summary>
        /// Rules that can be used to prove goals involving this predicate
        /// </summary>
        public readonly List<Rule> Rules = new List<Rule>();

        /// <summary>
        /// Add a new rule to the predicate
        /// </summary>
        /// <param name="r"></param>
        public void AddRule(Rule r)
        {
            if (IsPrimitive)
                throw new InvalidOperationException("Can't add rules to a primitive predicate");
            Rules.Add(r);
        }

    }
}
using System;
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
    public abstract class Goal
    {
        /// <summary>
        /// Predicate being called
        /// </summary>
        public readonly Predicate Predicate;

        public readonly Term[] Arguments;
        
        /// <summary>
        /// Make a new goal object
        /// </summary>
        protected Goal(Predicate predicate, Term[] args)
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
        public void If(params Goal[] subgoals) => Predicate.AddRule(new Rule(this, subgoals));

        public static Goal operator !(Goal g) => Language.Not[g];

        /// <summary>
        /// Make a copy of this rule, replacing any arguments that appear in the Dictionary with their values in the hash table.
        /// This is only called from Rule.Copy().
        /// </summary>
        /// <param name="vars">Mapping used to selectively rewrite arguments</param>
        /// <returns></returns>
        internal InstantiatedGoal Instantiate(Dictionary<Term,Term>? vars) =>
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
        #endregion

        #region Solving
        public bool IsTrue => Prover.CanProve(this);

        public bool IsFalse => Prover.CanProve(this);

        public T SolveFor<T>(Var<T> v) => Prover.SolveFor(v, this);
        public List<T> SolveForAll<T>(Var<T> v) => Prover.SolveForAll(v, this);

        public abstract IEnumerable<object> SolutionsUntyped { get; }
        #endregion
    }

    //
    // Goal objects with parametric types
    // Goals represent calls to predicates
    //
    // These goals have Terms as arguments; Terms are typed wrappers for variables and normal C# values
    // When they get called, constants get unwrapped into the underlying C# objects.
    //

    public class Goal<T1> : Goal
    {
        public Goal(Predicate predicate, Term<T1> arg1) : base(predicate, new Term[] { arg1 })
        {
        }

        public IEnumerable<T1> Solutions =>
            Prover.SolveAll<T1>(this, b => Unifier.DereferenceToConstant<T1>(Arguments[0], b));

        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    public class Goal<T1, T2> : Goal
    {
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2) : base(predicate, new Term[] { arg1, arg2 })
        {
        }

        public IEnumerable<(T1,T2)> Solutions =>
            Prover.SolveAll<(T1,T2)>(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b)));

        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    public class Goal<T1, T2, T3> : Goal
    {
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3) : base(predicate, new Term[] { arg1, arg2, arg3 })
        {
        }

        public IEnumerable<(T1,T2,T3)> Solutions =>
            Prover.SolveAll<(T1,T2,T3)>(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b),
                    Unifier.DereferenceToConstant<T3>(Arguments[2], b)));

        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    public class Goal<T1, T2, T3, T4> : Goal
    {
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4) : base(predicate, new Term[] { arg1, arg2, arg3, arg4 })
        {
        }

        public IEnumerable<(T1,T2,T3,T4)> Solutions =>
            Prover.SolveAll<(T1,T2,T3,T4)>(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b),
                    Unifier.DereferenceToConstant<T3>(Arguments[2], b),
                    Unifier.DereferenceToConstant<T4>(Arguments[3], b)));

        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    public class Goal<T1, T2, T3, T4, T5> : Goal
    {
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5) : base(predicate, new Term[] { arg1, arg2, arg3, arg4, arg5 })
        {
        }

        public IEnumerable<(T1,T2,T3,T4,T5)> Solutions =>
            Prover.SolveAll<(T1,T2,T3,T4,T5)>(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b),
                    Unifier.DereferenceToConstant<T3>(Arguments[2], b),
                    Unifier.DereferenceToConstant<T4>(Arguments[3], b),
                    Unifier.DereferenceToConstant<T5>(Arguments[4], b)));

        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    public class Goal<T1, T2, T3, T4, T5, T6> : Goal
    {
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5, Term<T6> arg6) : base(predicate, new Term[] { arg1, arg2, arg3, arg4, arg5, arg6 })
        {
        }

        public IEnumerable<(T1,T2,T3,T4,T5,T6)> Solutions =>
            Prover.SolveAll<(T1,T2,T3,T4,T5,T6)>(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b),
                    Unifier.DereferenceToConstant<T3>(Arguments[2], b),
                    Unifier.DereferenceToConstant<T4>(Arguments[3], b),
                    Unifier.DereferenceToConstant<T5>(Arguments[4], b),
                    Unifier.DereferenceToConstant<T6>(Arguments[5], b)));

        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    public class VariadicGoal<T> : Goal
    {
        // ReSharper disable once CoVariantArrayConversion
        public VariadicGoal(Predicate predicate, Term<T>[] argList) : base(predicate, argList)
        {
        }

        public override IEnumerable<object> SolutionsUntyped
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }

}

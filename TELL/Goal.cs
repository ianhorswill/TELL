using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TELL.Interpreter;
using static TELL.Interpreter.Prover;

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

        /// <summary>
        /// Arguments to the predicate
        /// </summary>
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

        /// <summary>
        /// Returns the negation of the specified goal
        /// This will succeed if the goal fails, and fail if the goal succeeds.
        /// </summary>
        /// <param name="g">Goal to negate</param>
        /// <returns>Negated goal</returns>
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
        /// <summary>
        /// Test if this goal is provable
        /// </summary>
        public bool IsTrue => CanProve(this);

        /// <summary>
        /// Using a goal as a Boolean is interpreted as calling it and returning true if it succeeds.
        /// </summary>
        public static implicit operator bool(Goal g) => g.IsTrue;

        /// <summary>
        /// Test if this goal is provable, return false if so, and true if not.
        /// </summary>
        public bool IsFalse => CanProve(this);

        /// <summary>
        /// Find the first solution to this goal and return the value of v from within it
        /// </summary>
        /// <typeparam name="T">Type of the variable we're solving for</typeparam>
        /// <param name="v">Variable whose value we should return</param>
        /// <returns>Value of the variable in the solution</returns>
        public T SolveFor<T>(Var<T> v) => Prover.SolveFor(v, this);

        /// <summary>
        /// Find all solutions to this goal, and collect the values of the specified variable in each
        /// </summary>
        /// <typeparam name="T">Value of the variable we're solving for</typeparam>
        /// <param name="v">Variable to solve for</param>
        /// <returns>All values found, including duplicate values</returns>
        public List<T> SolveForAll<T>(Var<T> v) => Prover.SolveForAll(v, this);

        /// <summary>
        /// All solutions to this goal, as a sequence of tuples, each giving the argument values from one solution
        /// </summary>
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

    /// <summary>
    /// A goal (predicate call) for a one-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's argument</typeparam>
    public class Goal<T1> : Goal
    {
        /// <summary>
        /// Make a goal for a predicate with a given argument
        /// </summary>
        public Goal(Predicate predicate, Term<T1> arg1) : base(predicate, new Term[] { arg1 })
        {
        }

        /// <summary>
        /// Find all solutions and return the values of the argument for each
        /// </summary>
        public IEnumerable<T1> Solutions =>
            SolveAll(this, b => Unifier.DereferenceToConstant<T1>(Arguments[0], b));

        /// <inheritdoc />
        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    /// <summary>
    /// A goal (predicate call) for a two-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    public class Goal<T1, T2> : Goal
    {
        /// <summary>
        /// Make a goal for a predicate with given arguments
        /// </summary>
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2) : base(predicate, new Term[] { arg1, arg2 })
        {
        }

        /// <summary>
        /// Find all solutions and return the values of the arguments for each as a sequence of tuples
        /// </summary>
        public IEnumerable<(T1,T2)> Solutions =>
            SolveAll(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b)));

        /// <inheritdoc />
        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    /// <summary>
    /// A goal (predicate call) for a three-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
    public class Goal<T1, T2, T3> : Goal
    {
        /// <summary>
        /// Make a goal for a predicate with given arguments
        /// </summary>
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3) : base(predicate, new Term[] { arg1, arg2, arg3 })
        {
        }

        /// <summary>
        /// Find all solutions and return the values of the arguments for each as a sequence of tuples
        /// </summary>
        public IEnumerable<(T1,T2,T3)> Solutions =>
            SolveAll(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b),
                    Unifier.DereferenceToConstant<T3>(Arguments[2], b)));

        /// <inheritdoc />
        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    /// <summary>
    /// A goal (predicate call) for a four-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
    /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
    public class Goal<T1, T2, T3, T4> : Goal
    {
        /// <summary>
        /// Make a goal for a predicate with given arguments
        /// </summary>
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4) : base(predicate, new Term[] { arg1, arg2, arg3, arg4 })
        {
        }

        /// <summary>
        /// Find all solutions and return the values of the arguments for each as a sequence of tuples
        /// </summary>
        public IEnumerable<(T1,T2,T3,T4)> Solutions =>
            SolveAll(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b),
                    Unifier.DereferenceToConstant<T3>(Arguments[2], b),
                    Unifier.DereferenceToConstant<T4>(Arguments[3], b)));

        /// <inheritdoc />
        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    /// <summary>
    /// A goal (predicate call) for a five-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
    /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
    /// <typeparam name="T5">Type of the predicate's fifth argument</typeparam>
    public class Goal<T1, T2, T3, T4, T5> : Goal
    {
        /// <summary>
        /// Make a goal for a predicate with given arguments
        /// </summary>
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5) : base(predicate, new Term[] { arg1, arg2, arg3, arg4, arg5 })
        {
        }

        /// <summary>
        /// Find all solutions and return the values of the arguments for each as a sequence of tuples
        /// </summary>
        public IEnumerable<(T1,T2,T3,T4,T5)> Solutions =>
            SolveAll(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b),
                    Unifier.DereferenceToConstant<T3>(Arguments[2], b),
                    Unifier.DereferenceToConstant<T4>(Arguments[3], b),
                    Unifier.DereferenceToConstant<T5>(Arguments[4], b)));

        /// <inheritdoc />
        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    /// <summary>
    /// A goal (predicate call) for a six-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
    /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
    /// <typeparam name="T5">Type of the predicate's fifth argument</typeparam>
    /// <typeparam name="T6">Type of the predicate's sixth argument</typeparam>
    public class Goal<T1, T2, T3, T4, T5, T6> : Goal
    {
        /// <summary>
        /// Make a goal for a predicate with given arguments
        /// </summary>
        public Goal(Predicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5, Term<T6> arg6) : base(predicate, new Term[] { arg1, arg2, arg3, arg4, arg5, arg6 })
        {
        }

        /// <summary>
        /// Find all solutions and return the values of the arguments for each as a sequence of tuples
        /// </summary>
        public IEnumerable<(T1,T2,T3,T4,T5,T6)> Solutions =>
            SolveAll(this, b =>
                (Unifier.DereferenceToConstant<T1>(Arguments[0], b),
                    Unifier.DereferenceToConstant<T2>(Arguments[1], b),
                    Unifier.DereferenceToConstant<T3>(Arguments[2], b),
                    Unifier.DereferenceToConstant<T4>(Arguments[3], b),
                    Unifier.DereferenceToConstant<T5>(Arguments[4], b),
                    Unifier.DereferenceToConstant<T6>(Arguments[5], b)));

        /// <inheritdoc />
        public override IEnumerable<object> SolutionsUntyped => Solutions.Cast<object>();
    }

    /// <summary>
    /// A goal for a variadic predicate (one that can take a variable number of arguments)
    /// </summary>
    /// <typeparam name="T">Type of the arguments</typeparam>
    public class VariadicGoal<T> : Goal
    {
        /// <summary>
        /// Make a new goal given a predicate and argument list
        /// </summary>
        // ReSharper disable once CoVariantArrayConversion
        public VariadicGoal(Predicate predicate, Term<T>[] argList) : base(predicate, argList)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<object> SolutionsUntyped => throw new NotImplementedException();
    }

}

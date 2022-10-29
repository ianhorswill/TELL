using System;
using System.Collections.Generic;
using static TELL.Prover;

namespace TELL
{
    /// <summary>
    /// Primitive predicates
    /// These are implemented directly in C# code rather than through interpreted rules
    /// </summary>
    public static class Primitives
    {
        /// <summary>
        /// Not[Goal]
        /// True if Goal is false (not provable)
        /// </summary>
        public static readonly TellPredicate<AnyGoal> Not = new TellPredicate<AnyGoal>("Not",
            (g, s, k) =>
            {
                var goalArgument = (InstantiatedGoal)g.Arguments[0]!;
                var goalArgumentTrue = goalArgument.Prove(s, _ => true);
                return !goalArgumentTrue && k(s);
            });

        /// <summary>
        /// Same(x,y) - note parens rather than []s
        /// True if x and y can be unified.  So it really means "true if they can be made the same"
        /// </summary>
        /// <typeparam name="T">Type of arguments</typeparam>
        public static AnyGoal Same<T>(Term<T> x, Term<T> y) =>
            new TellPredicate<T,T>("Same",
                (g, s, k) => Unifier.Unify(g.Arguments[0], g.Arguments[1], s, out var newS) && k(newS))[x,y];

        /// <summary>
        /// Different(x,y) - note parens rather than []s
        /// True if x and y cannot be unified.  So it really means "true if they can't be made the same"
        /// </summary>
        /// <typeparam name="T">Type of arguments</typeparam>
        public static AnyGoal Different<T>(Term<T> x, Term<T> y) =>
            new TellPredicate<T,T>("Same",
                (g, s, k) => !Unifier.Unify(g.Arguments[0], g.Arguments[1], s, out _) && k(s))[x,y];

        /// <summary>
        /// Member(elt, list)    - note ()s rather than []s
        /// True if elt is in list.
        /// </summary>
        /// <typeparam name="T">Type of list elements</typeparam>
        public static Goal<T, IList<T>> Member<T>(Term<T> element, Term<IList<T>> list) =>
            new TellPredicate<T, IList<T>>("member",
                ModeDispatch<T, IList<T>>("member",
                    (e, l) => l.Contains(e),
                    null,
                    l => l,
                    null))[element, list];

        /// <summary>
        /// Utility function to do the case analysis for one-argument predicates
        /// </summary>
        /// <typeparam name="T">Argument type for the predicate</typeparam>
        /// <param name="name">Name of the predicate (For error messages)</param>
        /// <param name="inMode">Implementation for when a value is passed in to the predicate rather than an unbound variable</param>
        /// <param name="outMode">Implementation for when the input is an unbound variable</param>
        public static PredicateImplementation ModeDispatch<T>(string name, Predicate<T>? inMode, Func<IEnumerable<T>>? outMode) =>
            (g, s, k) =>
            {
                var arg = Unifier.Dereference(g.Arguments[0], s);
                if (arg is T tValue)
                {
                    if (inMode == null)
                        throw new ArgumentException($"{name} called with an instantiated variable");
                    return inMode(tValue) && k(s);
                }
                // arg is a variable
                if (outMode == null)
                    throw new ArgumentException($"{name} called with an uninstantiated variable");
                foreach (var output in outMode())
                    if (Unifier.Unify(arg, output, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        /// <summary>
        /// Utility function to do the case analysis for two-argument predicates
        /// </summary>
        /// <typeparam name="T1">C# type of the first argument to the predicate</typeparam>
        /// <typeparam name="T2">C# type of the second argument to the predicate</typeparam>
        /// <param name="name">Name of the predicate (for error messages)</param>
        /// <param name="inInMode">Implementation for when both arguments are instantiated</param>
        /// <param name="inOutMode">Implementation for when only the first argument is instantiated</param>
        /// <param name="outInMode">Implementation for when only the second argument is instantiated</param>
        /// <param name="outOutMode">Implementation for when neither argument is instantiated</param>
        public static PredicateImplementation ModeDispatch<T1, T2>(string name,
            Func<T1, T2, bool>? inInMode,
            Func<T1, IEnumerable<T2>>? inOutMode,
            Func<T2, IEnumerable<T1>>? outInMode,
            Func<IEnumerable<(T1, T2)>>? outOutMode) =>
            (g, s, k) =>
            {
                void InstantiationException() =>
                    throw new ArgumentException($"Invalid instantiation of arguments in {g}");
                var arg1 = Unifier.Dereference(g.Arguments[0], s);
                var arg2 = Unifier.Dereference(g.Arguments[1], s);
                if (arg1 is T1 t1Value)
                {
                    if (arg2 is T2 t2Value)
                    {
                        if (inInMode == null)
                            InstantiationException();
                        return inInMode!(t1Value, t2Value) && k(s);
                    }
                    // arg2 is uninstantiated
                    if (inOutMode == null)
                        InstantiationException();
                    foreach (var output in inOutMode!(t1Value))
                        if (Unifier.Unify(arg2, output, s, out var newS) && k(newS))
                            return true;
                    return false;
                }
                // arg1 is a variable
                if (arg2 is T2 t2Value2)
                {
                    if (outInMode == null)
                        InstantiationException();
                    foreach (var output in outInMode!(t2Value2))
                        if (Unifier.Unify(arg1, output, s, out var newS) && k(newS))
                            return true;
                    return false;
                }
                // arg2 is uninstantiated
                if (outOutMode == null)
                    InstantiationException();
                foreach (var (t1, t2) in outOutMode!())
                    if (Unifier.Unify(arg1, t1, s, out var newS) && Unifier.Unify(arg2, t2, newS, out var finalS) && k(finalS))
                        return true;
                return false;
            };

        public static VariadicPredicate<AnyGoal> And = new VariadicPredicate<AnyGoal>("and",
            (g, s, k) => RunBody(g.Arguments, 0, s, k));

        private static bool RunBody(object?[] subgoals, int index, Substitution? s, SuccessContinuation k)
            => index < subgoals.Length
                ? ((InstantiatedGoal)subgoals[index]!).Prove(s, n => RunBody(subgoals, index + 1, n, k))
                : k(s);

    }
}

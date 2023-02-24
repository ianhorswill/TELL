using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using static TELL.Prover;

namespace TELL
{
    /// <summary>
    /// Primitive predicates
    /// These are implemented directly in C# code rather than through interpreted rules
    /// </summary>
    public static class Language
    {
        public static Term<T> Constant<T>(T value) => new Constant<T>(value);

        #region Convenience functions
        public static Predicate<T1> Predicate<T1>(string name, Var<T1> v) => new Predicate<T1>(name, v);

        public static Predicate<T1,T2> Predicate<T1,T2>(string name, Var<T1> v1, Var<T2> v2)
            => new Predicate<T1,T2>(name, v1, v2);

        public static Predicate<T1,T2,T3> Predicate<T1,T2,T3>(string name, Var<T1> v1, Var<T2> v2, Var<T3> v3) 
            => new Predicate<T1,T2,T3>(name, v1, v2,v3);

        public static Predicate<T1,T2,T3,T4> Predicate<T1,T2,T3,T4>(string name, Var<T1> v1, Var<T2> v2, Var<T3> v3, Var<T4> v4) 
            => new Predicate<T1,T2,T3,T4>(name, v1, v2, v3, v4);
        
        public static Predicate<T1,T2,T3,T4,T5> Predicate<T1,T2,T3,T4,T5>(string name, Var<T1> v1, Var<T2> v2, Var<T3> v3, Var<T4> v4, Var<T5> v5) 
            => new Predicate<T1,T2,T3,T4,T5>(name, v1, v2, v3, v4, v5);

        public static Predicate<T1,T2,T3,T4,T5,T6> Predicate<T1,T2,T3,T4,T5,T6>(string name, Var<T1> v1, Var<T2> v2, Var<T3> v3, Var<T4> v4, Var<T5> v5, Var<T6> v6) 
            => new Predicate<T1,T2,T3,T4,T5,T6>(name, v1, v2, v3, v4, v5, v6);

        public static Predicate<T1> Predicate<T1>(string name, PredicateImplementation implementation) =>
            new Predicate<T1>(name, implementation);

        public static Predicate<T1,T2> Predicate<T1,T2>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2>(name, implementation);

        public static Predicate<T1,T2,T3> Predicate<T1,T2,T3>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2,T3>(name, implementation);

        public static Predicate<T1,T2,T3,T4> Predicate<T1,T2,T3,T4>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2,T3,T4>(name, implementation);

        public static Predicate<T1,T2,T3,T4,T5> Predicate<T1,T2,T3,T4,T5>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2,T3,T4,T5>(name, implementation);

        public static Predicate<T1,T2,T3,T4,T5,T6> Predicate<T1,T2,T3,T4,T5,T6>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2,T3,T4,T5,T6>(name, implementation);

        public static Predicate<TOut> Predicate<TOut>(string name, Enumerator<TOut> func)
            => Predicate<TOut>(name, EnumeratorWrapper(func));

        public static Predicate<TIn> Predicate<TIn>(string name, SimpleTest<TIn> func)
            => Predicate<TIn>(name, TestWrapper(func));

        public static Predicate<TIn1, TIn2> Predicate<TIn1, TIn2>(string name, SimpleTest<TIn1, TIn2> func)
            => Predicate<TIn1, TIn2>(name, TestWrapper(func));

        public static Predicate<TIn1, TIn2, TIn3> Predicate<TIn1, TIn2, TIn3>(string name, SimpleTest<TIn1, TIn2, TIn3> func)
            => Predicate<TIn1, TIn2, TIn3>(name, TestWrapper(func));

        public static Predicate<TIn1, TIn2, TIn3, TIn4> Predicate<TIn1, TIn2, TIn3, TIn4>(string name, SimpleTest<TIn1, TIn2, TIn3, TIn4> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4>(name, TestWrapper(func));

        public static Predicate<TIn1, TIn2, TIn3, TIn4, TIn5> Predicate<TIn1, TIn2, TIn3, TIn4, TIn5>(string name, SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TIn5>(name, TestWrapper(func));

        public static Predicate<TIn, TOut> Predicate<TIn, TOut>(string name, Enumerator<TIn, TOut> func)
            => Predicate<TIn, TOut>(name, EnumeratorWrapper(func));

        public static Predicate<TIn1, TIn2, TOut> Predicate<TIn1, TIn2, TOut>(string name, Enumerator<TIn1, TIn2, TOut> func)
            => Predicate<TIn1, TIn2, TOut>(name, EnumeratorWrapper(func));

        public static Predicate<TIn1, TIn2, TIn3, TOut> Predicate<TIn1, TIn2, TIn3, TOut>(string name, Enumerator<TIn1, TIn2, TIn3, TOut> func)
            => Predicate<TIn1, TIn2, TIn3, TOut>(name, EnumeratorWrapper(func));

        public static Predicate<TIn1, TIn2, TIn3, TIn4, TOut> Predicate<TIn1, TIn2, TIn3, TIn4, TOut>(string name, Enumerator<TIn1, TIn2, TIn3, TIn4, TOut> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TOut>(name, EnumeratorWrapper(func));

        public static Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(string name, Enumerator<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(name, EnumeratorWrapper(func));

        public static Predicate<TOut> Predicate<TOut>(string name, Func<TOut> func, bool failOnNull = false)
            => Predicate<TOut>(name, FunctionWrapper(func, failOnNull));

        public static Predicate<TIn, TOut> Predicate<TIn, TOut>(string name, Func<TIn, TOut> func, bool failOnNull = false)
            => Predicate<TIn, TOut>(name, FunctionWrapper(func, failOnNull));

        public static Predicate<TIn1, TIn2, TOut> Predicate<TIn1, TIn2, TOut>(string name, Func<TIn1, TIn2, TOut> func, bool failOnNull = false)
            => Predicate<TIn1, TIn2, TOut>(name, FunctionWrapper(func, failOnNull));

        public static Predicate<TIn1, TIn2, TIn3, TOut> Predicate<TIn1, TIn2, TIn3, TOut>(string name, Func<TIn1, TIn2, TIn3, TOut> func, bool failOnNull = false)
            => Predicate<TIn1, TIn2, TIn3, TOut>(name, FunctionWrapper(func, failOnNull));

        public static Predicate<TIn1, TIn2, TIn3, TIn4, TOut> Predicate<TIn1, TIn2, TIn3, TIn4, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TOut> func, bool failOnNull = false)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TOut>(name, FunctionWrapper(func, failOnNull));

        public static Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func, bool failOnNull = false)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(name, FunctionWrapper(func, failOnNull));
        #endregion

        #region Primitive predicates
        /// <summary>
        /// Not[Goal]
        /// True if Goal is false (not provable)
        /// </summary>
        public static readonly Predicate<Goal> Not = new Predicate<Goal>("Not",
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
        public static Goal Same<T>(Term<T> x, Term<T> y) =>
            new Predicate<T,T>("Same",
                (g, s, k) => Unifier.Unify(g.Arguments[0], g.Arguments[1], s, out var newS) && k(newS))[x,y];

        /// <summary>
        /// Different(x,y) - note parens rather than []s
        /// True if x and y cannot be unified.  So it really means "true if they can't be made the same"
        /// </summary>
        /// <typeparam name="T">Type of arguments</typeparam>
        public static Goal Different<T>(Term<T> x, Term<T> y) =>
            new Predicate<T,T>("Same",
                (g, s, k) => !Unifier.Unify(g.Arguments[0], g.Arguments[1], s, out _) && k(s))[x,y];

        /// <summary>
        /// Member(elt, list)    - note ()s rather than []s
        /// True if elt is in list.
        /// </summary>
        /// <typeparam name="T">Type of list elements</typeparam>
        public static Goal<T, IList<T>> Member<T>(Term<T> element, Term<IList<T>> list) =>
            new Predicate<T, IList<T>>("member",
                ModeDispatch<T, IList<T>>("member",
                    (e, l) => l.Contains(e),
                    null,
                    l => l,
                    null))[element, list];

        public static Goal Break<T>(Term<T> arg) => Predicate("Break", (SimpleTest<T>)(s =>
        {
            Debugger.Break();
            return true;
        }))[arg];
        #endregion

        #region Wrappers for predicate implementations
        /// <summary>
        /// Utility function to do the case analysis for one-argument predicates
        /// </summary>
        /// <typeparam name="T">Argument type for the predicate</typeparam>
        /// <param name="name">Name of the predicate (For error messages)</param>
        /// <param name="inMode">Implementation for when a value is passed in to the predicate rather than an unbound variable</param>
        /// <param name="outMode">Implementation for when the input is an unbound variable</param>
        public static PredicateImplementation ModeDispatch<T>(string name, System.Predicate<T>? inMode, Func<IEnumerable<T>>? outMode) =>
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

        public static PredicateImplementation FunctionWrapper<TOut>(Func<TOut> func, bool failOnNull = false)
            => (g, s, k) =>
            {
                var outVar = g.Arguments[0];
                var result = func();
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        public static PredicateImplementation FunctionWrapper<TIn, TOut>(Func<TIn, TOut> func, bool failOnNull = false)
            => (g, s, k) =>
            {
                var dereference = Unifier.Dereference(g.Arguments[0], s);
                var inArg = (TIn)dereference!;
                var outVar = g.Arguments[1];
                var result = func(inArg);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        public static PredicateImplementation FunctionWrapper<TIn1, TIn2, TOut>(Func<TIn1, TIn2, TOut> func, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var outVar = g.Arguments[2];
                var result = func(inArg1, inArg2);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        public static PredicateImplementation FunctionWrapper<TIn1, TIn2, TIn3, TOut>(Func<TIn1, TIn2, TIn3, TOut> func, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var outVar = g.Arguments[3];
                var result = func(inArg1, inArg2, inArg3);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        public static PredicateImplementation FunctionWrapper<TIn1, TIn2, TIn3, TIn4, TOut>(Func<TIn1, TIn2, TIn3, TIn4, TOut> func, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                var outVar = g.Arguments[4];
                var result = func(inArg1, inArg2, inArg3, inArg4);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        public static PredicateImplementation FunctionWrapper<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                var inArg5 = (TIn5)Unifier.Dereference(g.Arguments[4], s)!;
                var outVar = g.Arguments[5];
                var result = func(inArg1, inArg2, inArg3, inArg4, inArg5);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        public delegate bool SimpleTest<TIn>(TIn arg);
        public static PredicateImplementation TestWrapper<TIn>(SimpleTest<TIn> func)
            => (g, s, k) =>
            {
                var inArg = (TIn)Unifier.Dereference(g.Arguments[0], s)!;
                var result = func(inArg);
                return result && k(s);
            };

        public delegate bool SimpleTest<TIn1, TIn2>(TIn1 arg1, TIn2 arg2);
        public static PredicateImplementation TestWrapper<TIn1, TIn2>(SimpleTest<TIn1, TIn2> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var result = func(inArg1, inArg2);
                return result && k(s);
            };

        public delegate bool SimpleTest<TIn1, TIn2, TIn3>(TIn1 arg1, TIn2 arg2, TIn3 arg3);
        public static PredicateImplementation TestWrapper<TIn1, TIn2, TIn3>(SimpleTest<TIn1, TIn2, TIn3> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var result = func(inArg1, inArg2, inArg3);
                return result && k(s);
            };

        public delegate bool SimpleTest<TIn1, TIn2, TIn3, TIn4>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4);
        public static PredicateImplementation TestWrapper<TIn1, TIn2, TIn3, TIn4>(SimpleTest<TIn1, TIn2, TIn3, TIn4> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                var result = func(inArg1, inArg2, inArg3, inArg4);
                return result && k(s);
            };

        public delegate bool SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4, TIn5 arg5);
        public static PredicateImplementation TestWrapper<TIn1, TIn2, TIn3, TIn4, TIn5>(SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                var inArg5 = (TIn5)Unifier.Dereference(g.Arguments[4], s)!;
                var result = func(inArg1, inArg2, inArg3, inArg4, inArg5);
                return result && k(s);
            };

        public delegate bool SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4, TIn5 arg5, TIn6 arg6);
        public static PredicateImplementation TestWrapper<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                var inArg5 = (TIn5)Unifier.Dereference(g.Arguments[4], s)!;
                var inArg6 = (TIn6)Unifier.Dereference(g.Arguments[5], s)!;
                var result = func(inArg1, inArg2, inArg3, inArg4, inArg5, inArg6);
                return result && k(s);
            };

        public delegate IEnumerable<TOut> Enumerator<TOut>();
        public static PredicateImplementation EnumeratorWrapper<TOut>(Enumerator<TOut> func)
            => (g, s, k) =>
            {
                var outVar = g.Arguments[0];
                var results = func();

                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        public delegate IEnumerable<TOut> Enumerator<TIn, TOut>(TIn arg);
        public static PredicateImplementation EnumeratorWrapper<TIn, TOut>(Enumerator<TIn, TOut> func)
            => (g, s, k) =>
            {
                var inArg = (TIn)Unifier.Dereference(g.Arguments[0], s)!;
                var outVar = g.Arguments[1];
                var results = func(inArg);

                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        public delegate IEnumerable<TOut> Enumerator<TIn1, TIn2, TOut>(TIn1 arg1, TIn2 arg2);
        public static PredicateImplementation EnumeratorWrapper<TIn1, TIn2, TOut>(Enumerator<TIn1, TIn2, TOut> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var outVar = g.Arguments[2];
                var results = func(inArg1, inArg2);
                
                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        public delegate IEnumerable<TOut> Enumerator<TIn1, TIn2, TIn3, TOut>(TIn1 arg1, TIn2 arg2, TIn3 arg3);
        public static PredicateImplementation EnumeratorWrapper<TIn1, TIn2, TIn3, TOut>(Enumerator<TIn1, TIn2, TIn3, TOut> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var outVar = g.Arguments[3];
                var results = func(inArg1, inArg2, inArg3);

                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        public delegate IEnumerable<TOut> Enumerator<TIn1, TIn2, TIn3, TIn4, TOut>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4);
        public static PredicateImplementation EnumeratorWrapper<TIn1, TIn2, TIn3, TIn4, TOut>(Enumerator<TIn1, TIn2, TIn3, TIn4, TOut> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                var outVar = g.Arguments[4];
                var results = func(inArg1, inArg2, inArg3, inArg4);
                
                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        public delegate IEnumerable<TOut> Enumerator<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4, TIn5 arg5);
        public static PredicateImplementation EnumeratorWrapper<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(Enumerator<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                var inArg5 = (TIn5)Unifier.Dereference(g.Arguments[4], s)!;
                var outVar = g.Arguments[5];
                var results = func(inArg1, inArg2, inArg3, inArg4, inArg5);
                
                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };
        #endregion

        public static VariadicPredicate<Goal> And = new VariadicPredicate<Goal>("and",
            (g, s, k) => RunBody(g.Arguments, 0, s, k));

        private static bool RunBody(object?[] subgoals, int index, Substitution? s, SuccessContinuation k)
            => index < subgoals.Length
                ? ((InstantiatedGoal)subgoals[index]!).Prove(s, n => RunBody(subgoals, index + 1, n, k))
                : k(s);
    }
}

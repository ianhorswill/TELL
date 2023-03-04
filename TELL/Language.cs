using System;
using System.Collections.Generic;
using System.Diagnostics;
using TELL.Interpreter;
using static TELL.Interpreter.Prover;

namespace TELL
{
    /// <summary>
    /// Primitive predicates
    /// These are implemented directly in C# code rather than through interpreted rules
    /// </summary>
    public static class Language
    {
        /// <summary>
        /// Change a C# value into a constant term.  All arguments to goals have to be passed as terms.
        /// Normally, C# values are converted to terms automatically by implicit conversions, but once in
        /// a while, C# type inference fails and you have to do it manually.
        /// </summary>
        /// <typeparam name="T">Type of the term</typeparam>
        /// <param name="value">Value to be passed to the predicate</param>
        /// <returns></returns>
        public static Term<T> Constant<T>(T value) => new Constant<T>(value);

        #region Convenience functions
        /// <summary>
        /// Make a new predicate to be defined by a set of If() rules.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="v">Variable representing the first argument.</param>
        public static Predicate<T1> Predicate<T1>(string name, Var<T1> v) => new Predicate<T1>(name, v);

        /// <summary>
        /// Make a new predicate to be defined by a set of If() rules.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="v1">Variable representing the first argument.</param>
        /// <param name="v2">Variable representing the second argument.</param>
        public static Predicate<T1,T2> Predicate<T1,T2>(string name, Var<T1> v1, Var<T2> v2)
            => new Predicate<T1,T2>(name, v1, v2);

        /// <summary>
        /// Make a new predicate to be defined by a set of If() rules.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="v1">Variable representing the first argument.</param>
        /// <param name="v2">Variable representing the second argument.</param>
        /// <param name="v3">Variable representing the third argument.</param>
        public static Predicate<T1,T2,T3> Predicate<T1,T2,T3>(string name, Var<T1> v1, Var<T2> v2, Var<T3> v3) 
            => new Predicate<T1,T2,T3>(name, v1, v2,v3);

        /// <summary>
        /// Make a new predicate to be defined by a set of If() rules.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="v1">Variable representing the first argument.</param>
        /// <param name="v2">Variable representing the second argument.</param>
        /// <param name="v3">Variable representing the third argument.</param>
        /// <param name="v4">Variable representing the fourth argument.</param>
        public static Predicate<T1,T2,T3,T4> Predicate<T1,T2,T3,T4>(string name, Var<T1> v1, Var<T2> v2, Var<T3> v3, Var<T4> v4) 
            => new Predicate<T1,T2,T3,T4>(name, v1, v2, v3, v4);
        
        /// <summary>
        /// Make a new predicate to be defined by a set of If() rules.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="T5">Type of the predicate's fifth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="v1">Variable representing the first argument.</param>
        /// <param name="v2">Variable representing the second argument.</param>
        /// <param name="v3">Variable representing the third argument.</param>
        /// <param name="v4">Variable representing the fourth argument.</param>
        /// <param name="v5">Variable representing the fifth argument.</param>
        public static Predicate<T1,T2,T3,T4,T5> Predicate<T1,T2,T3,T4,T5>(string name, Var<T1> v1, Var<T2> v2, Var<T3> v3, Var<T4> v4, Var<T5> v5) 
            => new Predicate<T1,T2,T3,T4,T5>(name, v1, v2, v3, v4, v5);

        /// <summary>
        /// Make a new predicate to be defined by a set of If() rules.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="T5">Type of the predicate's fifth argument</typeparam>
        /// <typeparam name="T6">Type of the predicate's fifth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="v1">Variable representing the first argument.</param>
        /// <param name="v2">Variable representing the second argument.</param>
        /// <param name="v3">Variable representing the third argument.</param>
        /// <param name="v4">Variable representing the fourth argument.</param>
        /// <param name="v5">Variable representing the fifth argument.</param>
        /// <param name="v6">Variable representing the sixth argument.</param>
        public static Predicate<T1,T2,T3,T4,T5,T6> Predicate<T1,T2,T3,T4,T5,T6>(string name, Var<T1> v1, Var<T2> v2, Var<T3> v3, Var<T4> v4, Var<T5> v5, Var<T6> v6) 
            => new Predicate<T1,T2,T3,T4,T5,T6>(name, v1, v2, v3, v4, v5, v6);

        /// <summary>
        /// Make a new predicate to be defined by a continuation-passing C# method.
        /// This is the most complicated, but most general way to make a primitive predicate.
        /// If you're implementing a simple function or enumerator, use one of specialized methods
        /// for implementing those.  Otherwise, it's easier to make an implementation function by
        /// calling the ModeDispatch function.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="implementation">C# code that implements the driver for this predicate</param>
        public static Predicate<T1> Predicate<T1>(string name, PredicateImplementation implementation) =>
            new Predicate<T1>(name, implementation);

        /// <summary>
        /// Make a new predicate to be defined by a continuation-passing C# method.
        /// This is the most complicated, but most general way to make a primitive predicate.
        /// If you're implementing a simple function or enumerator, use one of specialized methods
        /// for implementing those.  Otherwise, it's easier to make an implementation function by
        /// calling the ModeDispatch function.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="implementation">C# code that implements the driver for this predicate</param>
        public static Predicate<T1,T2> Predicate<T1,T2>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2>(name, implementation);

        /// <summary>
        /// Make a new predicate to be defined by a continuation-passing C# method.
        /// This is the most complicated, but most general way to make a primitive predicate.
        /// If you're implementing a simple function or enumerator, use one of specialized methods
        /// for implementing those.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="implementation">C# code that implements the driver for this predicate</param>
        public static Predicate<T1,T2,T3> Predicate<T1,T2,T3>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2,T3>(name, implementation);

        /// <summary>
        /// Make a new predicate to be defined by a continuation-passing C# method.
        /// This is the most complicated, but most general way to make a primitive predicate.
        /// If you're implementing a simple function or enumerator, use one of specialized methods
        /// for implementing those.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="implementation">C# code that implements the driver for this predicate</param>
        public static Predicate<T1,T2,T3,T4> Predicate<T1,T2,T3,T4>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2,T3,T4>(name, implementation);

        /// <summary>
        /// Make a new predicate to be defined by a continuation-passing C# method.
        /// This is the most complicated, but most general way to make a primitive predicate.
        /// If you're implementing a simple function or enumerator, use one of specialized methods
        /// for implementing those.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="T5">Type of the predicate's fifth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="implementation">C# code that implements the driver for this predicate</param>
        public static Predicate<T1,T2,T3,T4,T5> Predicate<T1,T2,T3,T4,T5>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2,T3,T4,T5>(name, implementation);

        /// <summary>
        /// Make a new predicate to be defined by a continuation-passing C# method.
        /// This is the most complicated, but most general way to make a primitive predicate.
        /// If you're implementing a simple function or enumerator, use one of specialized methods
        /// for implementing those.
        /// </summary>
        /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="T5">Type of the predicate's fifth argument</typeparam>
        /// <typeparam name="T6">Type of the predicate's fifth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="implementation">C# code that implements the driver for this predicate</param>
        public static Predicate<T1,T2,T3,T4,T5,T6> Predicate<T1,T2,T3,T4,T5,T6>(string name, PredicateImplementation implementation) =>
            new Predicate<T1,T2,T3,T4,T5,T6>(name, implementation);

        /// <summary>
        /// Make a new predicate defined by a C# Boolean function.
        /// This predicate cannot be used to enumerate argument values that makes it true; it must be called
        /// with all its inputs defined
        /// </summary>
        /// <typeparam name="TIn">Type of the predicate's first argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function that takes the values of the arguments and returns true or false</param>
        public static Predicate<TIn> Predicate<TIn>(string name, SimpleTest<TIn> func)
            => Predicate<TIn>(name, TestWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# Boolean function.
        /// This predicate cannot be used to enumerate argument values that makes it true; it must be called
        /// with all its inputs defined
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function that takes the values of the arguments and returns true or false</param>
        public static Predicate<TIn1, TIn2> Predicate<TIn1, TIn2>(string name, SimpleTest<TIn1, TIn2> func)
            => Predicate<TIn1, TIn2>(name, TestWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# Boolean function.
        /// This predicate cannot be used to enumerate argument values that makes it true; it must be called
        /// with all its inputs defined
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function that takes the values of the arguments and returns true or false</param>
        public static Predicate<TIn1, TIn2, TIn3> Predicate<TIn1, TIn2, TIn3>(string name, SimpleTest<TIn1, TIn2, TIn3> func)
            => Predicate<TIn1, TIn2, TIn3>(name, TestWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# Boolean function.
        /// This predicate cannot be used to enumerate argument values that makes it true; it must be called
        /// with all its inputs defined
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TIn4">Type of the predicate's fourth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function that takes the values of the arguments and returns true or false</param>
        public static Predicate<TIn1, TIn2, TIn3, TIn4> Predicate<TIn1, TIn2, TIn3, TIn4>(string name, SimpleTest<TIn1, TIn2, TIn3, TIn4> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4>(name, TestWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# Boolean function.
        /// This predicate cannot be used to enumerate argument values that makes it true; it must be called
        /// with all its inputs defined
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TIn4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="TIn5">Type of the predicate's fifth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function that takes the values of the arguments and returns true or false</param>
        public static Predicate<TIn1, TIn2, TIn3, TIn4, TIn5> Predicate<TIn1, TIn2, TIn3, TIn4, TIn5>(string name, SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TIn5>(name, TestWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# Boolean function.
        /// This predicate cannot be used to enumerate argument values that makes it true; it must be called
        /// with all its inputs defined
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TIn4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="TIn5">Type of the predicate's fifth argument</typeparam>
        /// <typeparam name="TIn6">Type of the predicate's fifth argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function that takes the values of the arguments and returns true or false</param>
        public static Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(string name, SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(name, TestWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# function that enumerates possible values for its
        /// argument.
        /// </summary>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function that enumerates possible values of the predicate's argument</param>
        public static Predicate<TOut> Predicate<TOut>(string name, Enumerator<TOut> func)
            => Predicate<TOut>(name, EnumeratorWrapper(func));

        /// <summary>
        /// Make a new predicate defined by a C# function that enumerates possible values for the last
        /// (output) argument give the values of the other (input) arguments.
        /// </summary>
        /// <typeparam name="TIn">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the possible values of the output argument</param>
        public static Predicate<TIn, TOut> Predicate<TIn, TOut>(string name, Enumerator<TIn, TOut> func)
            => Predicate<TIn, TOut>(name, EnumeratorWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# function that enumerates possible values for the last
        /// (output) argument give the values of the other (input) arguments.
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the possible values of the output argument</param>
        public static Predicate<TIn1, TIn2, TOut> Predicate<TIn1, TIn2, TOut>(string name, Enumerator<TIn1, TIn2, TOut> func)
            => Predicate<TIn1, TIn2, TOut>(name, EnumeratorWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# function that enumerates possible values for the last
        /// (output) argument give the values of the other (input) arguments.
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the possible values of the output argument</param>
        public static Predicate<TIn1, TIn2, TIn3, TOut> Predicate<TIn1, TIn2, TIn3, TOut>(string name, Enumerator<TIn1, TIn2, TIn3, TOut> func)
            => Predicate<TIn1, TIn2, TIn3, TOut>(name, EnumeratorWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# function that enumerates possible values for the last
        /// (output) argument give the values of the other (input) arguments.
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TIn4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the possible values of the output argument</param>
        public static Predicate<TIn1, TIn2, TIn3, TIn4, TOut> Predicate<TIn1, TIn2, TIn3, TIn4, TOut>(string name, Enumerator<TIn1, TIn2, TIn3, TIn4, TOut> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TOut>(name, EnumeratorWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# function that enumerates possible values for the last
        /// (output) argument give the values of the other (input) arguments.
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TIn4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="TIn5">Type of the predicate's fifth argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the possible values of the output argument</param>
        public static Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(string name, Enumerator<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(name, EnumeratorWrapper(func, name));

        /// <summary>
        /// Make a new predicate defined by a C# function that computes a value for the last
        /// (output) argument give the values of the other (input) arguments.
        /// This does not enumerate any arguments.  When called, all arguments except the last
        /// must have known values, and the predicate deterministically computes a single output value.
        /// </summary>
        /// <typeparam name="TOut">Type of the predicate's argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Parameterless function to compute to the value of the output argument</param>
        /// <param name="failOnNull">If the value of func is null (or default(TOut)), then the predicate should fail rather than return null.</param>
        public static Predicate<TOut> Predicate<TOut>(string name, Func<TOut> func, bool failOnNull = false)
            => Predicate<TOut>(name, FunctionWrapper(func, failOnNull));

        /// <summary>
        /// Make a new predicate defined by a C# function that computes a value for the last
        /// (output) argument give the values of the other (input) arguments.
        /// This does not enumerate any arguments.  When called, all arguments except the last
        /// must have known values, and the predicate deterministically computes a single output value.
        /// </summary>
        /// <typeparam name="TIn">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the value of the output argument</param>
        /// <param name="failOnNull">If the value of func is null (or default(TOut)), then the predicate has no value for the inputs and so should fail</param>
        public static Predicate<TIn, TOut> Predicate<TIn, TOut>(string name, Func<TIn, TOut> func, bool failOnNull = false)
            => Predicate<TIn, TOut>(name, FunctionWrapper(func, name, failOnNull));

        /// <summary>
        /// Make a new predicate defined by a C# function that computes a value for the last
        /// (output) argument give the values of the other (input) arguments.
        /// This does not enumerate any arguments.  When called, all arguments except the last
        /// must have known values, and the predicate deterministically computes a single output value.
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the value of the output argument</param>
        /// <param name="failOnNull">If the value of func is null (or default(TOut)), then the predicate has no value for the inputs and so should fail</param>
        public static Predicate<TIn1, TIn2, TOut> Predicate<TIn1, TIn2, TOut>(string name, Func<TIn1, TIn2, TOut> func, bool failOnNull = false)
            => Predicate<TIn1, TIn2, TOut>(name, FunctionWrapper(func, name, failOnNull));

        /// <summary>
        /// Make a new predicate defined by a C# function that computes a value for the last
        /// (output) argument give the values of the other (input) arguments.
        /// This does not enumerate any arguments.  When called, all arguments except the last
        /// must have known values, and the predicate deterministically computes a single output value.
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the value of the output argument</param>
        /// <param name="failOnNull">If the value of func is null (or default(TOut)), then the predicate has no value for the inputs and so should fail</param>
        public static Predicate<TIn1, TIn2, TIn3, TOut> Predicate<TIn1, TIn2, TIn3, TOut>(string name, Func<TIn1, TIn2, TIn3, TOut> func, bool failOnNull = false)
            => Predicate<TIn1, TIn2, TIn3, TOut>(name, FunctionWrapper(func, name, failOnNull));

        /// <summary>
        /// Make a new predicate defined by a C# function that computes a value for the last
        /// (output) argument give the values of the other (input) arguments.
        /// This does not enumerate any arguments.  When called, all arguments except the last
        /// must have known values, and the predicate deterministically computes a single output value.
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TIn4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the value of the output argument</param>
        /// <param name="failOnNull">If the value of func is null (or default(TOut)), then the predicate has no value for the inputs and so should fail</param>
        public static Predicate<TIn1, TIn2, TIn3, TIn4, TOut> Predicate<TIn1, TIn2, TIn3, TIn4, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TOut> func, bool failOnNull = false)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TOut>(name, FunctionWrapper(func, name, failOnNull));

        /// <summary>
        /// Make a new predicate defined by a C# function that computes a value for the last
        /// (output) argument give the values of the other (input) arguments.
        /// This does not enumerate any arguments.  When called, all arguments except the last
        /// must have known values, and the predicate deterministically computes a single output value.
        /// </summary>
        /// <typeparam name="TIn1">Type of the predicate's first argument</typeparam>
        /// <typeparam name="TIn2">Type of the predicate's second argument</typeparam>
        /// <typeparam name="TIn3">Type of the predicate's third argument</typeparam>
        /// <typeparam name="TIn4">Type of the predicate's fourth argument</typeparam>
        /// <typeparam name="TIn5">Type of the predicate's fifth argument</typeparam>
        /// <typeparam name="TOut">Type of the predicate's last (output) argument</typeparam>
        /// <param name="name">Name of the predicate.</param>
        /// <param name="func">Function from the input arguments to the value of the output argument</param>
        /// <param name="failOnNull">If the value of func is null (or default(TOut)), then the predicate has no value for the inputs and so should fail</param>
        public static Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func, bool failOnNull = false)
            => Predicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(name, FunctionWrapper(func, name, failOnNull));
        #endregion

        #region Primitive predicates
        /// <summary>
        /// Not[Goal]
        /// True if Goal is false (not provable)
        /// </summary>
        public static readonly Predicate<Goal> Not = new Predicate<Goal>("Not",
            (g, s, k) =>
            {
                var goalArgument = Unifier.DereferenceToConstant<InstantiatedGoal>(g.Arguments[0], s, nameof(Not), 1);
                var goalArgumentTrue = goalArgument.Prove(s, _ => true);
                return !goalArgumentTrue && k(s);
            });

        /// <summary>
        /// Runs the goal predicate, but succeeds only once; does not look for further solutions if we backtrack.
        /// </summary>
        public static readonly Predicate<Goal> Once = new Predicate<Goal>("Once",
            (g, s, k) =>
            {
                Substitution? result = null;
                var goalArgument = Unifier.DereferenceToConstant<InstantiatedGoal>(g.Arguments[0], s, nameof(Once), 1);
                var goalArgumentTrue = goalArgument.Prove(s, finalSubst =>
                {
                    result = finalSubst;
                    return true;
                });
                return goalArgumentTrue && k(result);
            });

        /// <summary>
        /// True when the last argument is the sum, across all solutions to the goal, of the first argument.
        /// </summary>
        public static readonly Predicate<float, Goal, float> Sum = new Predicate<float,Goal,float>("Sum",
            (g, s, k) =>
            {
                float result = 0;
                var goalArgument = Unifier.DereferenceToConstant<InstantiatedGoal>(g.Arguments[1], s, nameof(Sum), 2);
                goalArgument.Prove(s, solution =>
                {
                    result += Unifier.DereferenceToConstant<float>(g.Arguments[0], solution, nameof(Sum), 1);
                    return false;
                });
                return Unifier.Unify(result, g.Arguments[2], s, out var finalSubst) && k(finalSubst);
            });

        /// <summary>
        /// True when the argument is an unbound variable
        /// This is still true if the argument is a variable that has been unified with another variable,
        /// provided that other variable is also unbound.
        /// </summary>
        public static Goal Unbound<T>(Term<T> arg)
            => new Predicate<T>("Bound", (g, s, k) => Unifier.Dereference(g.Arguments[0], s) is Var<T> && k(s))[arg];

        /// <summary>
        /// True when the argument is not an unbound variable, i.e. is either a constant of a bound variable
        /// </summary>
        public static Goal Bound<T>(Term<T> arg)
            => new Predicate<T>("Bound", (g, s, k) => Unifier.Dereference(g.Arguments[0], s) is T && k(s))[arg];

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

        /// <summary>
        /// Breakpoint: executing this drops the program in the debugger so you can see the vlaue of arg.
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="arg">Argument.  Break ignores the argument, but passing something here gives you a convenient way to inspect the object while the program is running.</param>
        public static Goal Break<T>(Term<T> arg) => Predicate("Break", (SimpleTest<T>)(s =>
        {
            Debugger.Break();
            return true;
        }))[arg];

        /// <summary>
        /// True when all of its argument goals are true
        /// </summary>
        public static VariadicPredicate<Goal> And = new VariadicPredicate<Goal>("And",
            (g, s, k) => RunBody(g.Arguments, 0, s, k));

        private static bool RunBody(object?[] subgoals, int index, Substitution? s, SuccessContinuation k)
            => index < subgoals.Length
                ? ((InstantiatedGoal)subgoals[index]!).Prove(s, n => RunBody(subgoals, index + 1, n, k))
                : k(s);

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

        /// <summary>
        /// Converts a C# Func to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation FunctionWrapper<TOut>(Func<TOut> func, bool failOnNull = false)
            => (g, s, k) =>
            {
                var outVar = g.Arguments[0];
                var result = func();
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        /// <summary>
        /// Converts a C# Func to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation FunctionWrapper<TIn, TOut>(Func<TIn, TOut> func, string predicateName, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg = Unifier.DereferenceToConstant<TIn>(g.Arguments[0], s, predicateName, 1)!;
                var outVar = g.Arguments[1];
                var result = func(inArg);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        /// <summary>
        /// Converts a C# Func to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation FunctionWrapper<TIn1, TIn2, TOut>(Func<TIn1, TIn2, TOut> func, string predicateName, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var outVar = g.Arguments[2];
                var result = func(inArg1, inArg2);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        /// <summary>
        /// Converts a C# Func to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation FunctionWrapper<TIn1, TIn2, TIn3, TOut>(Func<TIn1, TIn2, TIn3, TOut> func, string predicateName, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var outVar = g.Arguments[3];
                var result = func(inArg1, inArg2, inArg3);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        /// <summary>
        /// Converts a C# Func to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation FunctionWrapper<TIn1, TIn2, TIn3, TIn4, TOut>(Func<TIn1, TIn2, TIn3, TIn4, TOut> func, string predicateName, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var inArg4 = Unifier.DereferenceToConstant<TIn4>(g.Arguments[3], s, predicateName, 4)!;
                var outVar = g.Arguments[4];
                var result = func(inArg1, inArg2, inArg3, inArg4);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        /// <summary>
        /// Converts a C# Func to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation FunctionWrapper<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func, string predicateName, bool failOnNull = false)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var inArg4 = Unifier.DereferenceToConstant<TIn4>(g.Arguments[3], s, predicateName, 4)!;
                var inArg5 = Unifier.DereferenceToConstant<TIn5>(g.Arguments[4], s, predicateName, 5)!;
                var outVar = g.Arguments[5];
                var result = func(inArg1, inArg2, inArg3, inArg4, inArg5);
                if (failOnNull && result == null)
                    return false;
                return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
            };

        /// <summary>
        /// A function that implements a predicate that just tests its arguments without
        /// filling any of them in.
        /// </summary>
        public delegate bool SimpleTest<TIn>(TIn arg);
        
        /// <summary>
        /// Converts a C# predicate, in the sense of function from arguments to a Boolean, to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation TestWrapper<TIn>(SimpleTest<TIn> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn>(g.Arguments[0], s, predicateName, 1)!;
                var result = func(inArg1);
                return result && k(s);
            };

        /// <summary>
        /// A function that implements a predicate that just tests its arguments without
        /// filling any of them in.
        /// </summary>
        public delegate bool SimpleTest<TIn1, TIn2>(TIn1 arg1, TIn2 arg2);
        
        /// <summary>
        /// Converts a C# predicate, in the sense of function from arguments to a Boolean, to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation TestWrapper<TIn1, TIn2>(SimpleTest<TIn1, TIn2> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var result = func(inArg1, inArg2);
                return result && k(s);
            };

        /// <summary>
        /// A function that implements a predicate that just tests its arguments without
        /// filling any of them in.
        /// </summary>
        public delegate bool SimpleTest<TIn1, TIn2, TIn3>(TIn1 arg1, TIn2 arg2, TIn3 arg3);
        
        /// <summary>
        /// Converts a C# predicate, in the sense of function from arguments to a Boolean, to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation TestWrapper<TIn1, TIn2, TIn3>(SimpleTest<TIn1, TIn2, TIn3> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var result = func(inArg1, inArg2, inArg3);
                return result && k(s);
            };

        /// <summary>
        /// A function that implements a predicate that just tests its arguments without
        /// filling any of them in.
        /// </summary>
        public delegate bool SimpleTest<TIn1, TIn2, TIn3, TIn4>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4);
        
        /// <summary>
        /// Converts a C# predicate, in the sense of function from arguments to a Boolean, to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation TestWrapper<TIn1, TIn2, TIn3, TIn4>(SimpleTest<TIn1, TIn2, TIn3, TIn4> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var inArg4 = Unifier.DereferenceToConstant<TIn4>(g.Arguments[3], s, predicateName, 4)!;
                var result = func(inArg1, inArg2, inArg3, inArg4);
                return result && k(s);
            };

        /// <summary>
        /// A function that implements a predicate that just tests its arguments without
        /// filling any of them in.
        /// </summary>
        public delegate bool SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4, TIn5 arg5);
        
        /// <summary>
        /// Converts a C# predicate, in the sense of function from arguments to a Boolean, to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation TestWrapper<TIn1, TIn2, TIn3, TIn4, TIn5>(SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var inArg4 = Unifier.DereferenceToConstant<TIn4>(g.Arguments[3], s, predicateName, 4)!;
                var inArg5 = Unifier.DereferenceToConstant<TIn5>(g.Arguments[4], s, predicateName, 5)!;
                var result = func(inArg1, inArg2, inArg3, inArg4, inArg5);
                return result && k(s);
            };

        /// <summary>
        /// A function that implements a predicate that just tests its arguments without
        /// filling any of them in.
        /// </summary>
        public delegate bool SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4, TIn5 arg5, TIn6 arg6);
        
        /// <summary>
        /// Converts a C# predicate, in the sense of function from arguments to a Boolean, to the driver code for a predicate
        /// </summary>
        public static PredicateImplementation TestWrapper<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6>(SimpleTest<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var inArg4 = Unifier.DereferenceToConstant<TIn4>(g.Arguments[3], s, predicateName, 4)!;
                var inArg5 = Unifier.DereferenceToConstant<TIn5>(g.Arguments[4], s, predicateName, 5)!;
                var inArg6 = Unifier.DereferenceToConstant<TIn6>(g.Arguments[5], s, predicateName, 6)!;
                var result = func(inArg1, inArg2, inArg3, inArg4, inArg5, inArg6);
                return result && k(s);
            };

        
        /// <summary>
        /// A function that implements a predicate that enumerates values of its (only) argument.
        /// </summary>
        public delegate IEnumerable<TOut> Enumerator<TOut>();
        
        /// <summary>
        /// Makes a driver function that implements a predicate that enumerates values of its (only) argument.
        /// </summary>
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

        /// <summary>
        /// A function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public delegate IEnumerable<TOut> Enumerator<TIn, TOut>(TIn arg);
        
        /// <summary>
        /// Makes a driver function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public static PredicateImplementation EnumeratorWrapper<TIn, TOut>(Enumerator<TIn, TOut> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg = Unifier.DereferenceToConstant<TIn>(g.Arguments[0], s, predicateName, 1)!;
                var outVar = g.Arguments[1];
                var results = func(inArg);

                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        /// <summary>
        /// A function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public delegate IEnumerable<TOut> Enumerator<TIn1, TIn2, TOut>(TIn1 arg1, TIn2 arg2);
        
        /// <summary>
        /// Makes a driver function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public static PredicateImplementation EnumeratorWrapper<TIn1, TIn2, TOut>(Enumerator<TIn1, TIn2, TOut> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var outVar = g.Arguments[2];
                var results = func(inArg1, inArg2);
                
                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        /// <summary>
        /// A function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public delegate IEnumerable<TOut> Enumerator<TIn1, TIn2, TIn3, TOut>(TIn1 arg1, TIn2 arg2, TIn3 arg3);
        
        /// <summary>
        /// Makes a driver function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public static PredicateImplementation EnumeratorWrapper<TIn1, TIn2, TIn3, TOut>(Enumerator<TIn1, TIn2, TIn3, TOut> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName,1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s)!;
                var outVar = g.Arguments[3];
                var results = func(inArg1, inArg2, inArg3);

                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        /// <summary>
        /// A function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public delegate IEnumerable<TOut> Enumerator<TIn1, TIn2, TIn3, TIn4, TOut>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4);
        
        /// <summary>
        /// Makes a driver function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public static PredicateImplementation EnumeratorWrapper<TIn1, TIn2, TIn3, TIn4, TOut>(Enumerator<TIn1, TIn2, TIn3, TIn4, TOut> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var inArg4 = Unifier.DereferenceToConstant<TIn4>(g.Arguments[3], s, predicateName, 4)!;
                var outVar = g.Arguments[4];
                var results = func(inArg1, inArg2, inArg3, inArg4);
                
                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };

        /// <summary>
        /// A function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public delegate IEnumerable<TOut> Enumerator<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(TIn1 arg1, TIn2 arg2, TIn3 arg3, TIn4 arg4, TIn5 arg5);
        
        /// <summary>
        /// Makes a driver function that implements a predicate that enumerates values of its last argument
        /// given values of its other arguments.
        /// </summary>
        public static PredicateImplementation EnumeratorWrapper<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>(Enumerator<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func, string predicateName)
            => (g, s, k) =>
            {
                var inArg1 = (TIn1)Unifier.DereferenceToConstant<TIn1>(g.Arguments[0], s, predicateName, 1)!;
                var inArg2 = (TIn2)Unifier.DereferenceToConstant<TIn2>(g.Arguments[1], s, predicateName, 2)!;
                var inArg3 = (TIn3)Unifier.DereferenceToConstant<TIn3>(g.Arguments[2], s, predicateName, 3)!;
                var inArg4 = (TIn4)Unifier.DereferenceToConstant<TIn4>(g.Arguments[3], s, predicateName, 4)!;
                var inArg5 = (TIn5)Unifier.DereferenceToConstant<TIn5>(g.Arguments[4], s, predicateName, 5)!;
                var outVar = g.Arguments[5];
                var results = func(inArg1, inArg2, inArg3, inArg4, inArg5);
                
                foreach (var result in results)
                    if (Unifier.Unify(outVar, result, s, out var newS) && k(newS))
                        return true;
                return false;
            };
        #endregion

    }
}

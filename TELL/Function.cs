using System;

namespace TELL
{
    /// <summary>
    /// Wrapper for a C# function that takes no inputs
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    public class Function<TOut> : TellPredicate<TOut>
    {
        /// <summary>
        /// Wrapper for a C# function that takes no inputs
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="func">C# function implementing the predicate</param>
        public Function(string name, Func<TOut> func)
            : base(name,
                (g, s, k) =>
                {
                    var outVar = g.Arguments[0];
                    var result = func();
                    return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
                })
        {}
    }

    /// <summary>
    /// Wrapper for a C# function
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TIn">Type of input</typeparam>
    public class Function<TIn, TOut> : TellPredicate<TIn, TOut>
    {
        /// <summary>
        /// Wrapper for a C# function
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="func">C# function implementing the predicate</param>
        public Function(string name, Func<TIn, TOut> func)
            : base(name,
                (g, s, k) =>
                {
                    var inArg = (TIn)Unifier.Dereference(g.Arguments[0], s)!;
                    var outVar = g.Arguments[1];
                    var result = func(inArg);
                    return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
                })
        {}
    }

    /// <summary>
    /// Wrapper for a C# function
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TIn1">Type of input</typeparam>
    /// <typeparam name="TIn2">Type of input</typeparam>
    public class Function<TIn1, TIn2, TOut> : TellPredicate<TIn1, TIn2, TOut>
    {
        /// <summary>
        /// Wrapper for a C# function
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="func">C# function implementing the predicate</param>
        public Function(string name, Func<TIn1, TIn2, TOut> func)
            : base(name,
                (g, s, k) =>
                {
                    var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                    var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                    var outVar = g.Arguments[2];
                    var result = func(inArg1, inArg2);
                    return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
                })
        {}
    }

    /// <summary>
    /// Wrapper for a C# function
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TIn1">Type of input</typeparam>
    /// <typeparam name="TIn2">Type of input</typeparam>
    /// <typeparam name="TIn3">Type of input</typeparam>
    public class Function<TIn1, TIn2, TIn3, TOut> : TellPredicate<TIn1, TIn2, TIn3, TOut>
    {
        /// <summary>
        /// Wrapper for a C# function
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="func">C# function implementing the predicate</param>
        public Function(string name, Func<TIn1, TIn2, TIn3, TOut> func)
            : base(name,
                (g, s, k) =>
                {
                    var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                    var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                    var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                    var outVar = g.Arguments[3];
                    var result = func(inArg1, inArg2, inArg3);
                    return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
                })
        {}
    }

    /// <summary>
    /// Wrapper for a C# function
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TIn1">Type of input</typeparam>
    /// <typeparam name="TIn2">Type of input</typeparam>
    /// <typeparam name="TIn3">Type of input</typeparam>
    /// <typeparam name="TIn4">Type of input</typeparam>
    public class Function<TIn1, TIn2, TIn3, TIn4, TOut> : TellPredicate<TIn1, TIn2, TIn3, TIn4, TOut>
    {
        /// <summary>
        /// Wrapper for a C# function
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="func">C# function implementing the predicate</param>
        public Function(string name, Func<TIn1, TIn2, TIn3, TIn4, TOut> func)
            : base(name,
                (g, s, k) =>
                {
                    var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                    var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                    var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                    var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                    var outVar = g.Arguments[4];
                    var result = func(inArg1, inArg2, inArg3, inArg4);
                    return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
                })
        {}
    }

    /// <summary>
    /// Wrapper for a C# function
    /// </summary>
    /// <typeparam name="TOut"></typeparam>
    /// <typeparam name="TIn1">Type of input</typeparam>
    /// <typeparam name="TIn2">Type of input</typeparam>
    /// <typeparam name="TIn3">Type of input</typeparam>
    /// <typeparam name="TIn4">Type of input</typeparam>
    /// <typeparam name="TIn5">Type of input</typeparam>
    public class Function<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> : TellPredicate<TIn1, TIn2, TIn3, TIn4, TIn5, TOut>
    {
        /// <summary>
        /// Wrapper for a C# function
        /// </summary>
        /// <param name="name">Name of the function</param>
        /// <param name="func">C# function implementing the predicate</param>
        public Function(string name, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TOut> func)
            : base(name,
                (g, s, k) =>
                {
                    var inArg1 = (TIn1)Unifier.Dereference(g.Arguments[0], s)!;
                    var inArg2 = (TIn2)Unifier.Dereference(g.Arguments[1], s)!;
                    var inArg3 = (TIn3)Unifier.Dereference(g.Arguments[2], s)!;
                    var inArg4 = (TIn4)Unifier.Dereference(g.Arguments[3], s)!;
                    var inArg5 = (TIn5)Unifier.Dereference(g.Arguments[4], s)!;
                    var outVar = g.Arguments[5];
                    var result = func(inArg1, inArg2, inArg3, inArg4, inArg5);
                    return Unifier.Unify(outVar, result, s, out var newS) && k(newS);
                })
        {}
    }
}

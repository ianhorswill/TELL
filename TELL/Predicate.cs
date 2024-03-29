﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using TELL.Interpreter;
using static TELL.Interpreter.Prover;
using static TELL.Interpreter.CsvReader;

namespace TELL
{
    /// <summary>
    /// Represents a predicate you can call as a goal.
    /// A predicate has a name, and a set of rules for when it can be true.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public abstract class Predicate
    {
        /// <summary>
        /// Human-readable name for this predicate
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Documentation, if provided
        /// </summary>
        public string? DocString;

        /// <summary>
        /// The name, arguments, and documentation for this predicate
        /// </summary>
        public string ManualEntry()
        {
            var b = new StringBuilder();
            b.Append(Name);
            b.Append("[");
            var first = true;
            foreach (var arg in DefaultVariables)
            {
                if (first)
                    first = false;
                else
                    b.Append(", ");
                b.Append(((IVariable)arg).VariableName);
            }

            b.Append("]");
            if (DocString != null)
                b.Append($": {DocString}");

            return b.ToString();
        }

        /// <summary>
        /// Code to call when trying to prove a goal involving this predicate
        /// </summary>
        public readonly PredicateImplementation Implementation;

        /// <summary>
        /// If true, this is a predicate directly implemented in C#/MSIL.  If false, it's defined by rules.
        /// </summary>
        public readonly bool IsPrimitive;

        /// <summary>
        /// Arguments to use for the goal is you say Predicate.If(body)
        /// For primitive predicates, which also gives the REPL a way of knowing the predicate's
        /// expected argument types.
        /// </summary>
        public readonly Term[] DefaultVariables;

        /// <summary>
        /// Make a new predicate
        /// </summary>
        protected Predicate(string name, PredicateImplementation implementation, params Term[] defaultVariables)
        {
            Name = name;
            Implementation = implementation;
            DefaultVariables = defaultVariables;
            IsPrimitive = implementation != ProveUsingRules;
            Program.MaybeAddPredicate(this);
        }

        /// <summary>
        /// Make a new predicate to be defined in terms of rules.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultVariables"></param>
        protected Predicate(string name, Term[] defaultVariables) : this(name, ProveUsingRules, defaultVariables)
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

        //public Goal this[params Term[] args] => GetGoal(args);
        
        /// <summary>
        /// Make a goal (call) to this predicate given an array of arguments
        /// </summary>
        /// <param name="args">Term expressions for the arguments</param>
        /// <returns>Goal object</returns>
        public abstract Goal GetGoal(Term[] args);

        /// <summary>
        /// Check that argument is instantiated to a value of the right type and return it caast to that type
        /// </summary>
        /// <param name="arg">Argument (variable or object)</param>
        /// <param name="argumentNumber">Position in argument list</param>
        /// <typeparam name="T">Expected type</typeparam>
        /// <returns>Value of type T</returns>
        /// <exception cref="ArgumentException">If arg is uninstantiated (unbound) or of the wrong type</exception>
        protected Term<T> CastArgument<T>(Term arg, int argumentNumber)
        {
            if (arg is Term<T> t) return t;
            throw new ArgumentException(
                $"Argument {argumentNumber} to {Name}, {arg}, was expected to be a {typeof(T).Name} but was a {arg.Type.Name}");
        }

        /// <summary>
        /// Verify that the header row of a CSV file matches the declared variable names
        /// </summary>
        protected void VerifyCsvColumnNames(string[] headerRow)
        {
            if (headerRow.Length != DefaultVariables.Length)
                throw new InvalidDataException(
                    $"Predicate {Name} declared with {DefaultVariables.Length} arguments, but CSV file contains {headerRow.Length} columns");
            for (var i = 0; i < headerRow.Length; i++)
            {
                var headerName = headerRow[i];
                var argumentName = ((IVariable)DefaultVariables[i]).VariableName;
                if (string.Compare(headerName, argumentName, true, CultureInfo.InvariantCulture) != 0)
                    throw new InvalidDataException(
                        $"For predicate {Name}, the column name {headerName} in the CSV file does not match the declared name {argumentName}");
            }
        }

        /// <summary>
        /// Load a set of facts from a CSV file
        /// </summary>
        /// <param name="path">path to the file</param>
        protected void LoadCsv(string path)
        {
            var (header, data) = ReadCsv(path);
            VerifyCsvColumnNames(header);
            foreach (var row in data)
                CsvRowToFact(row).Fact();
        }

        /// <summary>
        /// Convert a CSV row loaded from a file into a Goal that can be asserted as a fact.
        /// </summary>
        protected abstract Goal CsvRowToFact(string[] row);
    }
    //
    // Typed version of AnyPredicate
    // These are the equivalent of subroutines
    // They're called TellPredicate to about name collision with System.Predicate.
    //

    /// <summary>
    /// Single-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's argument</typeparam>
    public class Predicate<T1> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1> this[Term<T1> arg1] => new Goal<T1>(this, arg1);

        /// <inheritdoc />
        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException($"{Name} expects 1 argument but was given {args.Length}");
            return this[CastArgument<T1>(args[0], 1)];
        }

        /// <summary>
        /// Make a predicate for use with rules
        /// </summary>
        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }

        /// <summary>
        /// Make a new primitive predicate
        /// </summary>
        public Predicate(string name, PredicateImplementation i) : base(name, i, (Var<T1>)typeof(T1).Name)
        {
        }

        /// <summary>
        /// Add a rule to the predicate, using the default arguments as the rule's head (i.e. goal
        /// </summary>
        /// <param name="body">Subgoals to call to prove the head goal</param>
        /// <returns>Original predicate</returns>
        /// <exception cref="InvalidOperationException">If this is a primitive predicate</exception>
        public Predicate<T1> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }

        /// <inheritdoc />
        protected override Goal CsvRowToFact(string[] row) => this[ConvertCell<T1>(row[0])];

        /// <summary>
        /// Load a set of facts from a CSV file into this predicate.
        /// </summary>
        /// <param name="path">Path to the CSV file</param>
        /// <returns>this predicate</returns>
        public Predicate<T1> FromFile(string path)
        {
            LoadCsv(path);
            return this;
        }

        /// <summary>
        /// Add documentation to the predicate
        /// </summary>
        /// <param name="docString">Documentation to be stored with the predicate</param>
        /// <returns>Original predicate</returns>
        public Predicate<T1> Documentation(string docString)
        {
            DocString = docString;
            return this;
        }
    }

    /// <summary>
    /// Two-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    public class Predicate<T1, T2> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2> this[Term<T1> arg1, Term<T2> arg2] => new Goal<T1, T2>(this, arg1, arg2);

        /// <inheritdoc />
        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 2)
                throw new ArgumentException($"{Name} expects 2 arguments but was given {args.Length}");
            return this[CastArgument<T1>(args[0], 1), CastArgument<T2>(args[1], 2)];
        }

        /// <summary>
        /// Make a predicate for use with rules
        /// </summary>
        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }

        /// <summary>
        /// Make a new primitive predicate
        /// </summary>
        public Predicate(string name, PredicateImplementation i) : base(name, i, (Var<T1>)typeof(T1).Name, (Var<T2>)typeof(T2).Name)
        {
        }

        /// <summary>
        /// Add a rule to the predicate, using the default arguments as the rule's head (i.e. goal
        /// </summary>
        /// <param name="body">Subgoals to call to prove the head goal</param>
        /// <returns>Original predicate</returns>
        /// <exception cref="InvalidOperationException">If this is a primitive predicate</exception>
        public Predicate<T1,T2> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }

        /// <inheritdoc />
        protected override Goal CsvRowToFact(string[] row) 
            => this[ConvertCell<T1>(row[0]), ConvertCell<T2>(row[1])];

        /// <summary>
        /// Load a set of facts from a CSV file into this predicate.
        /// </summary>
        /// <param name="path">Path to the CSV file</param>
        /// <returns>this predicate</returns>
        public Predicate<T1,T2> FromFile(string path)
        {
            LoadCsv(path);
            return this;
        }

        /// <summary>
        /// Add documentation to the predicate
        /// </summary>
        /// <param name="docString">Documentation to be stored with the predicate</param>
        /// <returns>Original predicate</returns>
        public Predicate<T1,T2> Documentation(string docString)
        {
            DocString = docString;
            return this;
        }
    }

    /// <summary>
    /// Three-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
    public class Predicate<T1, T2, T3> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3] => new Goal<T1, T2, T3>(this, arg1, arg2,arg3);

        /// <inheritdoc />
        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 3)
                throw new ArgumentException($"{Name} expects 3 arguments but was given {args.Length}");
            return this[CastArgument<T1>(args[0], 1), CastArgument<T2>(args[1], 2), CastArgument<T3>(args[2], 3)];
        }

        /// <summary>
        /// Make a predicate for use with rules
        /// </summary>
        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }
        
        /// <summary>
        /// Make a new primitive predicate
        /// </summary>
        public Predicate(string name, PredicateImplementation i) : base(name, i, (Var<T1>)typeof(T1).Name, (Var<T2>)typeof(T2).Name, (Var<T3>)typeof(T3).Name)
        {
        }

        /// <summary>
        /// Add a rule to the predicate, using the default arguments as the rule's head (i.e. goal
        /// </summary>
        /// <param name="body">Subgoals to call to prove the head goal</param>
        /// <returns>Original predicate</returns>
        /// <exception cref="InvalidOperationException">If this is a primitive predicate</exception>
        public Predicate<T1,T2,T3> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }

        /// <inheritdoc />
        protected override Goal CsvRowToFact(string[] row) 
            => this[ConvertCell<T1>(row[0]), ConvertCell<T2>(row[1]), ConvertCell<T3>(row[2])];

        /// <summary>
        /// Load a set of facts from a CSV file into this predicate.
        /// </summary>
        /// <param name="path">Path to the CSV file</param>
        /// <returns>this predicate</returns>
        public Predicate<T1,T2,T3> FromFile(string path)
        {
            LoadCsv(path);
            return this;
        }

        /// <summary>
        /// Add documentation to the predicate
        /// </summary>
        /// <param name="docString">Documentation to be stored with the predicate</param>
        /// <returns>Original predicate</returns>
        public Predicate<T1,T2,T3> Documentation(string docString)
        {
            DocString = docString;
            return this;
        }
    }

    /// <summary>
    /// Four-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
    /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
    public class Predicate<T1, T2, T3, T4> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4] 
            => new Goal<T1, T2, T3, T4>(this, arg1, arg2,arg3, arg4);

        /// <inheritdoc />
        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 4)
                throw new ArgumentException($"{Name} expects 4 arguments but was given {args.Length}");
            return this[CastArgument<T1>(args[0], 1), CastArgument<T2>(args[1], 2), CastArgument<T3>(args[2], 3), CastArgument<T4>(args[3], 4)];
        }

        /// <summary>
        /// Make a predicate for use with rules
        /// </summary>
        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }
        
        /// <summary>
        /// Make a new primitive predicate
        /// </summary>
        public Predicate(string name, PredicateImplementation i) : base(name, i, (Var<T1>)typeof(T1).Name, (Var<T2>)typeof(T2).Name, (Var<T3>)typeof(T3).Name, (Var<T4>)typeof(T4).Name)
        {
        }

        /// <summary>
        /// Add a rule to the predicate, using the default arguments as the rule's head (i.e. goal
        /// </summary>
        /// <param name="body">Subgoals to call to prove the head goal</param>
        /// <returns>Original predicate</returns>
        /// <exception cref="InvalidOperationException">If this is a primitive predicate</exception>
        public Predicate<T1,T2,T3,T4> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }

        /// <inheritdoc />
        protected override Goal CsvRowToFact(string[] row) 
            => this[ConvertCell<T1>(row[0]), ConvertCell<T2>(row[1]), ConvertCell<T3>(row[2]),
                    ConvertCell<T4>(row[3])];
        
        /// <summary>
        /// Load a set of facts from a CSV file into this predicate.
        /// </summary>
        /// <param name="path">Path to the CSV file</param>
        /// <returns>this predicate</returns>
        public Predicate<T1,T2,T3,T4> FromFile(string path)
        {
            LoadCsv(path);
            return this;
        }

        /// <summary>
        /// Add documentation to the predicate
        /// </summary>
        /// <param name="docString">Documentation to be stored with the predicate</param>
        /// <returns>Original predicate</returns>
        public Predicate<T1,T2,T3,T4> Documentation(string docString)
        {
            DocString = docString;
            return this;
        }
    }

    /// <summary>
    /// Five-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
    /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
    /// <typeparam name="T5">Type of the predicate's fifth argument</typeparam>
    public class Predicate<T1, T2, T3, T4, T5> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4, T5> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5] 
            => new Goal<T1, T2, T3, T4, T5>(this, arg1, arg2,arg3, arg4, arg5);

        /// <inheritdoc />
        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 5)
                throw new ArgumentException($"{Name} expects 5 arguments but was given {args.Length}");
            return this[CastArgument<T1>(args[0], 1), CastArgument<T2>(args[1], 2), CastArgument<T3>(args[2], 3),
                        CastArgument<T4>(args[3], 4), CastArgument<T5>(args[4], 5)];
        }

        /// <summary>
        /// Make a predicate for use with rules
        /// </summary>
        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }
        
        /// <summary>
        /// Make a new primitive predicate
        /// </summary>
        public Predicate(string name, PredicateImplementation i) : base(name, i, (Var<T1>)typeof(T1).Name, (Var<T2>)typeof(T2).Name, (Var<T3>)typeof(T3).Name, (Var<T4>)typeof(T4).Name, (Var<T5>)typeof(T5).Name)
        {
        }

        /// <summary>
        /// Add a rule to the predicate, using the default arguments as the rule's head (i.e. goal
        /// </summary>
        /// <param name="body">Subgoals to call to prove the head goal</param>
        /// <returns>Original predicate</returns>
        /// <exception cref="InvalidOperationException">If this is a primitive predicate</exception>
        public Predicate<T1,T2,T3,T4,T5> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }

        /// <inheritdoc />
        protected override Goal CsvRowToFact(string[] row) 
            => this[ConvertCell<T1>(row[0]), ConvertCell<T2>(row[1]), ConvertCell<T3>(row[2]),
                ConvertCell<T4>(row[3]), ConvertCell<T5>(row[4])];
        
        /// <summary>
        /// Load a set of facts from a CSV file into this predicate.
        /// </summary>
        /// <param name="path">Path to the CSV file</param>
        /// <returns>this predicate</returns>
        public Predicate<T1,T2,T3,T4,T5> FromFile(string path)
        {
            LoadCsv(path);
            return this;
        }

        /// <summary>
        /// Add documentation to the predicate
        /// </summary>
        /// <param name="docString">Documentation to be stored with the predicate</param>
        /// <returns>Original predicate</returns>
        public Predicate<T1,T2,T3,T4,T5> Documentation(string docString)
        {
            DocString = docString;
            return this;
        }

    }

    /// <summary>
    /// Six-argument predicate
    /// </summary>
    /// <typeparam name="T1">Type of the predicate's first argument</typeparam>
    /// <typeparam name="T2">Type of the predicate's second argument</typeparam>
    /// <typeparam name="T3">Type of the predicate's third argument</typeparam>
    /// <typeparam name="T4">Type of the predicate's fourth argument</typeparam>
    /// <typeparam name="T5">Type of the predicate's fifth argument</typeparam>
    /// <typeparam name="T6">Type of the predicate's sixth argument</typeparam>
    public class Predicate<T1, T2, T3, T4, T5, T6> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4, T5, T6> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5, Term<T6> arg6] 
            => new Goal<T1, T2, T3, T4, T5, T6>(this, arg1, arg2,arg3, arg4, arg5, arg6);

        /// <inheritdoc />
        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 6)
                throw new ArgumentException($"{Name} expects 6 arguments but was given {args.Length}");
            return this[CastArgument<T1>(args[0], 1), CastArgument<T2>(args[1], 2), CastArgument<T3>(args[2], 3),
                        CastArgument<T4>(args[3], 4), CastArgument<T5>(args[4], 5), CastArgument<T6>(args[5], 6)];
        }

        /// <summary>
        /// Make a predicate for use with rules
        /// </summary>
        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }

        /// <summary>
        /// Make a new primitive predicate
        /// </summary>
        public Predicate(string name, PredicateImplementation i) : base(name, i, (Var<T1>)typeof(T1).Name, (Var<T2>)typeof(T2).Name, (Var<T3>)typeof(T3).Name, (Var<T4>)typeof(T4).Name, (Var<T5>)typeof(T5).Name, (Var<T6>)typeof(T6).Name)
        {
        }

        /// <summary>
        /// Add a rule to the predicate, using the default arguments as the rule's head (i.e. goal
        /// </summary>
        /// <param name="body">Subgoals to call to prove the head goal</param>
        /// <returns>Original predicate</returns>
        /// <exception cref="InvalidOperationException">If this is a primitive predicate</exception>
        public Predicate<T1,T2,T3,T4,T5,T6> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }

        /// <inheritdoc />
        protected override Goal CsvRowToFact(string[] row) 
            => this[ConvertCell<T1>(row[0]), ConvertCell<T2>(row[1]), ConvertCell<T3>(row[2]),
                ConvertCell<T4>(row[3]), ConvertCell<T5>(row[4]), ConvertCell<T6>(row[5])];

        /// <summary>
        /// Load a set of facts from a CSV file into this predicate.
        /// </summary>
        /// <param name="path">Path to the CSV file</param>
        /// <returns>this predicate</returns>
        public Predicate<T1,T2,T3,T4,T5,T6> FromFile(string path)
        {
            LoadCsv(path);
            return this;
        }

        /// <summary>
        /// Add documentation to the predicate
        /// </summary>
        /// <param name="docString">Documentation to be stored with the predicate</param>
        /// <returns>Original predicate</returns>
        public Predicate<T1,T2,T3,T4,T5,T6> Documentation(string docString)
        {
            DocString = docString;
            return this;
        }

    }

    /// <summary>
    /// A predicate that takes a variable number of arguments, all of which are type T.
    /// </summary>
    /// <typeparam name="T">Type of the predicate's arguments</typeparam>
    public class VariadicPredicate<T> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public VariadicGoal<T> this[params Term<T>[] args] => new VariadicGoal<T>(this, args);

        /// <inheritdoc />
        public override Goal GetGoal(Term[] args) => this[args.Cast<Term<T>>().ToArray()];

        /// <inheritdoc />
        protected override Goal CsvRowToFact(string[] row) => throw new NotImplementedException();

        /// <summary>
        /// Make a new primitive predicate
        /// </summary>
        public VariadicPredicate(string name, PredicateImplementation i) : base(name, i)
        {
        }

        /// <summary>
        /// Add documentation to the predicate
        /// </summary>
        /// <param name="docString">Documentation to be stored with the predicate</param>
        /// <returns>Original predicate</returns>
        public VariadicPredicate<T> Documentation(string docString)
        {
            DocString = docString;
            return this;
        }
    }
}

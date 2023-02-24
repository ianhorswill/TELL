using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
        public readonly Prover.PredicateImplementation Implementation;
        public readonly bool IsPrimitive;
        public readonly Term[]? DefaultVariables;

        /// <summary>
        /// Make a new predicate
        /// </summary>
        protected Predicate(string name, Prover.PredicateImplementation implementation, Term[]? defaultVariables)
        {
            Name = name;
            Implementation = implementation;
            DefaultVariables = (defaultVariables != null && defaultVariables.Length>0)?defaultVariables:null;
            IsPrimitive = implementation != Prover.ProveUsingRules;
        }

        protected Predicate(string name, Term[]? defaultVariables = null) : this(name, Prover.ProveUsingRules, defaultVariables)
        {
        }

        protected Predicate(string name, Prover.PredicateImplementation implementation) : this(name, implementation,
            null)
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
        
        public abstract Goal GetGoal(Term[] args);
    }
    //
    // Typed version of AnyPredicate
    // These are the equivalent of subroutines
    // They're called TellPredicate to about name collision with System.Predicate.
    //

    public class Predicate<T1> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1> this[Term<T1> arg1] => new Goal<T1>(this, arg1);

        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException($"{Name} expects 1 argument but was given {args.Length}");
            return this[(Term<T1>)args[0]];
        }

        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }

        public Predicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }

        public Predicate<T1> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }
    }

    public class Predicate<T1, T2> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2> this[Term<T1> arg1, Term<T2> arg2] => new Goal<T1, T2>(this, arg1, arg2);

        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 2)
                throw new ArgumentException($"{Name} expects 2 arguments but was given {args.Length}");
            return this[(Term<T1>)args[0], (Term<T2>)args[1]];
        }

        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }

        public Predicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }

        public Predicate<T1,T2> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }
    }

    public class Predicate<T1, T2, T3> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3] => new Goal<T1, T2, T3>(this, arg1, arg2,arg3);

        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 3)
                throw new ArgumentException($"{Name} expects 3 arguments but was given {args.Length}");
            return this[(Term<T1>)args[0], (Term<T2>)args[1], (Term<T3>)args[2]];
        }

        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }
        public Predicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }

        public Predicate<T1,T2,T3> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }
    }

    public class Predicate<T1, T2, T3, T4> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4] 
            => new Goal<T1, T2, T3, T4>(this, arg1, arg2,arg3, arg4);

        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 4)
                throw new ArgumentException($"{Name} expects 4 arguments but was given {args.Length}");
            return this[(Term<T1>)args[0], (Term<T2>)args[1], (Term<T3>)args[2], (Term<T4>)args[3]];
        }

        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }
        public Predicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }

        public Predicate<T1,T2,T3,T4> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }
    }

    public class Predicate<T1, T2, T3, T4, T5> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4, T5> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5] 
            => new Goal<T1, T2, T3, T4, T5>(this, arg1, arg2,arg3, arg4, arg5);

        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 5)
                throw new ArgumentException($"{Name} expects 5 arguments but was given {args.Length}");
            return this[(Term<T1>)args[0], (Term<T2>)args[1], (Term<T3>)args[2], (Term<T4>)args[3], (Term<T5>)args[4]];
        }

        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }
        public Predicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }

        public Predicate<T1,T2,T3,T4,T5> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }
    }

    public class Predicate<T1, T2, T3, T4, T5, T6> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4, T5, T6> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5, Term<T6> arg6] 
            => new Goal<T1, T2, T3, T4, T5, T6>(this, arg1, arg2,arg3, arg4, arg5, arg6);

        public override Goal GetGoal(Term[] args)
        {
            if (args.Length != 6)
                throw new ArgumentException($"{Name} expects 6 arguments but was given {args.Length}");
            return this[(Term<T1>)args[0], (Term<T2>)args[1], (Term<T3>)args[2], (Term<T4>)args[3], (Term<T5>)args[4], (Term<T6>)args[5]];
        }

        public Predicate(string name, params Term[] args) : base(name, args)
        {
        }

        public Predicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }

        public Predicate<T1,T2,T3,T4,T5,T6> If(params Goal[] body)
        {
            if (DefaultVariables == null)
                throw new InvalidOperationException("Cannot add a rule to a primitive predicate");
            GetGoal(DefaultVariables).If(body);
            return this;
        }
    }

    public class VariadicPredicate<T> : Predicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public VariadicGoal<T> this[params Term<T>[] args] => new VariadicGoal<T>(this, args);

        public override Goal GetGoal(Term[] args) => this[args.Cast<Term<T>>().ToArray()];

        public VariadicPredicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }
    }
}

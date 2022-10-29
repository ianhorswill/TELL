namespace TELL
{
    //
    // Typed version of AnyPredicate
    // These are the equivalent of subroutines
    // They're called TellPredicate to about name collision with System.Predicate.
    //

    public class TellPredicate<T1> : AnyPredicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1> this[Term<T1> arg1] => new Goal<T1>(this, arg1);

        public TellPredicate(string name) : base(name)
        {
        }

        public TellPredicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }
    }

    public class TellPredicate<T1, T2> : AnyPredicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2> this[Term<T1> arg1, Term<T2> arg2] => new Goal<T1, T2>(this, arg1, arg2);

        public TellPredicate(string name) : base(name)
        {
        }
        public TellPredicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }
    }

    public class TellPredicate<T1, T2, T3> : AnyPredicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3] => new Goal<T1, T2, T3>(this, arg1, arg2,arg3);

        public TellPredicate(string name) : base(name)
        {
        }
        public TellPredicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }
    }

    public class TellPredicate<T1, T2, T3, T4> : AnyPredicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4] 
            => new Goal<T1, T2, T3, T4>(this, arg1, arg2,arg3, arg4);

        public TellPredicate(string name) : base(name)
        {
        }
        public TellPredicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }
    }

    public class TellPredicate<T1, T2, T3, T4, T5> : AnyPredicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4, T5> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5] 
            => new Goal<T1, T2, T3, T4, T5>(this, arg1, arg2,arg3, arg4, arg5);

        public TellPredicate(string name) : base(name)
        {
        }
        public TellPredicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }
    }

    public class TellPredicate<T1, T2, T3, T4, T5, T6> : AnyPredicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public Goal<T1, T2, T3, T4, T5, T6> this[Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5, Term<T6> arg6] 
            => new Goal<T1, T2, T3, T4, T5, T6>(this, arg1, arg2,arg3, arg4, arg5, arg6);

        public TellPredicate(string name) : base(name)
        {
        }
        public TellPredicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }
    }

    public class VariadicPredicate<T> : AnyPredicate
    {
        /// <summary>
        /// Make a Goal from this predicate with the specified argument value.
        /// </summary>
        public VariadicGoal<T> this[params Term<T>[] args] => new VariadicGoal<T>(this, args);

        public VariadicPredicate(string name, Prover.PredicateImplementation i) : base(name, i)
        {
        }
    }
}

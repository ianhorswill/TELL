namespace TELL
{
    //
    // Goal objects with parametric types
    // Goals represent calls to predicates
    //
    // These goals have Terms as arguments; Terms are typed wrappers for variables and normal C# values
    // When they get called, constants get unwrapped into the underlying C# objects.
    //

    public class Goal<T1> : AnyGoal
    {
        public Goal(AnyPredicate predicate, Term<T1> arg1) : base(predicate, new AnyTerm[] { arg1 })
        {
        }
    }

    public class Goal<T1, T2> : AnyGoal
    {
        public Goal(AnyPredicate predicate, Term<T1> arg1, Term<T2> arg2) : base(predicate, new AnyTerm[] { arg1, arg2 })
        {
        }
    }

    public class Goal<T1, T2, T3> : AnyGoal
    {
        public Goal(AnyPredicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3) : base(predicate, new AnyTerm[] { arg1, arg2, arg3 })
        {
        }
    }

    public class Goal<T1, T2, T3, T4> : AnyGoal
    {
        public Goal(AnyPredicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4) : base(predicate, new AnyTerm[] { arg1, arg2, arg3, arg4 })
        {
        }
    }

    public class Goal<T1, T2, T3, T4, T5> : AnyGoal
    {
        public Goal(AnyPredicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5) : base(predicate, new AnyTerm[] { arg1, arg2, arg3, arg4, arg5 })
        {
        }
    }

    public class Goal<T1, T2, T3, T4, T5, T6> : AnyGoal
    {
        public Goal(AnyPredicate predicate, Term<T1> arg1, Term<T2> arg2, Term<T3> arg3, Term<T4> arg4, Term<T5> arg5, Term<T6> arg6) : base(predicate, new AnyTerm[] { arg1, arg2, arg3, arg4, arg5, arg6 })
        {
        }
    }

    public class VariadicGoal<T> : AnyGoal
    {
        // ReSharper disable once CoVariantArrayConversion
        public VariadicGoal(AnyPredicate predicate, Term<T>[] argList) : base(predicate, argList)
        {
        }
    }

}

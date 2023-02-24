using System;
using System.Collections.Generic;

namespace TELL
{
    /// <summary>
    /// Base class of all Terms.  Terms are expressions representing arguments to predicates.
    /// </summary>
    public abstract class Term
    {
        /// <summary>
        /// We use this rather than an "is" test because there's no way given the C# type system
        /// to have a base class for all terms and also a base class for just the variables, but
        /// regardless of the type of the variable.  So we have to test whether something is a variable
        /// using a virtual function.  And unless microsoft has optimized their JIT compilation of
        /// type testing, this is faster anyway.
        /// </summary>
        /// <value></value>
        public abstract bool IsVariable { get; }

        /// <summary>
        /// Return the instantiated form of this term to place in an instantiated goal.
        /// See Rule.Instantiate for more info.
        /// </summary>
        /// <param name="vars">Variable mapping.  It's typed as AnyTerm because there's no base class for just the variables.</param>
        public abstract object? Instantiate(Dictionary<Term,Term>? vars);

        /// <summary>
        /// Used to clone variables; It's virtual method here because of typing issues.
        /// </summary>
        public virtual Term Clone() =>
            throw new NotImplementedException("Clone should only be called on variables, not constants");

        /// <summary>
        /// Type of this term
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// Make a variable of the same type as this term
        /// </summary>
        internal abstract Term MakeVariable(string name);
    }

    /// <summary>
    /// Base class for terms whose values are type T.
    /// </summary>
    /// <typeparam name="T">Type of the value of the variable</typeparam>
    public abstract class Term<T> : Term
    {
        /// <summary>
        /// Automatically convert C# constant of type T to Constant terms
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Term<T>(T value) => new Constant<T>(value);

        public static Goal operator ==(Term<T> t1, Term<T> t2) => Language.Same(t1, t2);

        public static Goal operator !=(Term<T> t1, Term<T> t2) => Language.Different(t1, t2);

        public override Type Type => typeof(T);

        internal override Term MakeVariable(string name) => new Var<T>(name);
    }
}

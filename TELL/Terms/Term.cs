using System;
using System.Collections.Generic;

namespace TELL
{
    /// <summary>
    /// Base class of all Terms.  Terms are expressions representing arguments to predicates.
    /// TELL is interpreted, and so its data structures are essentially parse trees of the code
    /// it's running. Terms represent the arguments to predicates within those data structures.
    /// Terms are typed and can either be variables (type Var[T]), in which case they're placeholders
    /// for values that will be determined at runtime, or constants (type Constant[T]), in which
    /// case they're fixed.
    /// </summary>
    public abstract class Term
    {
        /// <summary>
        /// True if this Term is a variable.
        /// This is faster than doing a type check for IVariable.
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
#pragma warning disable CS0660, CS0661
    public abstract class Term<T> : Term
#pragma warning restore CS0660, CS0661
    {
        /// <summary>
        /// Automatically convert C# constant of type T to Constant terms
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Term<T>(T value) => new Constant<T>(value);

        /// <summary>
        /// True when the terms have the same value
        /// </summary>
        public static Goal operator ==(Term<T> t1, Term<T> t2) => Language.Same(t1, t2);

        /// <summary>
        /// True when the terms have different values
        /// </summary>
        public static Goal operator !=(Term<T> t1, Term<T> t2) => Language.Different(t1, t2);

        /// <summary>
        /// Return the type object for this Term's type
        /// </summary>
        public override Type Type => typeof(T);

        /// <summary>
        /// Make a variable of the same type as this term, whether this term is a variable or not.
        /// </summary>
        /// <param name="name">Name to give to the variable</param>
        internal override Term MakeVariable(string name) => new Var<T>(name);
    }
}

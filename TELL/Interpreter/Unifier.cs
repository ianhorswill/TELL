using System;

namespace TELL.Interpreter
{
    /// <summary>
    /// Methods to implement unification of variables
    /// </summary>
    public static class Unifier
    {
        /// <summary>
        /// Return the final value of a variable within a substitution.  It's defined as follows (constant
        /// here just means anything other than a Variable):
        /// - Dereference(constant, subst) = constant
        /// - Dereference(variable, subst) = Dereference(variable's value in subst, subst)
        ///
        /// Note that this means Dereference will only ever return a constant or a variable that doesn't
        /// have a value in the substitution, aka an unbound variable.
        /// </summary>
        public static object? Dereference(object? constantOrVariable, Substitution? subst)
        {
            object? result = constantOrVariable;
            while (result is Term v && v.IsVariable && Substitution.Lookup(subst, v, out var vValue))
                result = vValue;

            return result;
        }

        /// <summary>
        /// Find the value of the first argument given the substitution, and cast it to the specified type
        /// </summary>
        /// <typeparam name="T">Expected type</typeparam>
        /// <param name="constantOrVariable">Value to dereference</param>
        /// <param name="subst">Substitution</param>
        /// <returns>Value</returns>
        /// <exception cref="Exception">If value is of the wrong type or is an unbound variable</exception>
        public static T DereferenceToConstant<T>(object? constantOrVariable, Substitution? subst)
        {
            switch (Dereference(constantOrVariable, subst))
            {
                case T t: return t;
                case Constant<T> c: return c.Value;  // In case we're called from Solutions, which can pass in the original term objects of the goal
                case Term v:
                    throw new Exception(
                        $"Variable {v} should have been bound to a value of type {typeof(T).Name}, but was unbound");

                default:
                    throw new Exception(
                        $"Value {constantOrVariable} should have been of type {typeof(T).Name}, but was not.");
            }
        }

        /// <summary>
        /// Find the value of the first argument given the substitution, and cast it to the specified type
        /// </summary>
        /// <typeparam name="T">Expected type</typeparam>
        /// <param name="constantOrVariable">Value to dereference</param>
        /// <param name="subst">Substitution</param>
        /// <param name="predicateName">Name of the predicate being executed</param>
        /// <param name="argNumber">Position of the argument to the predicate that we're dereferencing</param>
        /// <returns>Value</returns>
        /// <exception cref="Exception">If value is of the wrong type or is an unbound variable</exception>
        public static T DereferenceToConstant<T>(object? constantOrVariable, Substitution? subst, string predicateName, int argNumber)
        {
            switch (Dereference(constantOrVariable, subst))
            {
                case T t: return t;
                case Constant<T> c: return c.Value;  // In case we're called from Solutions, which can pass in the original term objects of the goal
                case Term v:
                    throw new Exception(
                        $"Argument {argNumber} to predicate {predicateName}, {v}, should have been bound to a value of type {typeof(T).Name}, but was unbound");

                default:
                    throw new Exception(
                        $"Value of argument {argNumber} to {predicateName}, {constantOrVariable}, should have been of type {typeof(T).Name}, but was not.");
            }
        }

        /// <summary>
        /// Test if it's possible to make the dereferenced versions of the two values the same, possibly by extending
        /// the substitution.  Output the Substitution that makes them the same.  If the dereferenced versions are already
        /// the same, then the unifiedSubst will just be the original one.  If they're not the same, but one of them is
        /// a variable, then the unifiedSubst will be the original subst extended with the variable set to the other
        /// dereferenced value.
        ///
        /// If the dereferenced values are different constants, they can't be made the same, and unify should return false.
        /// </summary>
        /// <param name="a">The first value to compare</param>
        /// <param name="b">The second value to compare</param>
        /// <param name="subst">The substitution currently in use</param>
        /// <param name="unifyingSubst">The extension of subst that makes a and b the same (this may just be the original subst)</param>
        /// <returns></returns>
        public static bool Unify(object? a, object? b, Substitution? subst, out Substitution? unifyingSubst)
        {
            a = Dereference(a, subst);
            b = Dereference(b, subst);

            unifyingSubst = subst;

            if (a == null || b == null)
                return ReferenceEquals(a, b);

            if (a.Equals(b))
                return true;
            if (a is Term va && va.IsVariable)
            {
                unifyingSubst = new Substitution(va, b, subst);
                return true;
            }
            if (b is Term vb && vb.IsVariable)
            {
                unifyingSubst = new Substitution(vb, a, subst);
                return true;
            }

            unifyingSubst = null;
            return false;
        }

        /// <summary>
        /// Check if a[0] can be unified with b[0], a[1] with b[1], etc.  Output the substitution needed to make
        /// each pair unify.
        /// </summary>
        public static bool UnifyArrays(object?[] a, object?[] b, Substitution? subst, out Substitution? unifyingSubst)
        {
            unifyingSubst = subst;
            if (a.Length != b.Length)
                return false;

            for (var i = 0; i < a.Length; i++)
                if (!Unify(a[i], b[i], unifyingSubst, out unifyingSubst))
                    return false;
            return true;
        }
    }
}

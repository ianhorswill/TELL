namespace TELL
{
    /// <summary>
    /// Untyped base interface to for all Var[T]T.
    /// THE ONLY CLASS THAT SHOULD IMPLEMENT THIS IS Var[T]!  Right now, it's only used to ask a variable
    /// what it's name is (you can't access the Name field without casting it to Var[T], which you can't
    /// do if you don't know T at compile time).
    /// </summary>
    public interface IVariable
    {
        /// <summary>
        /// Name of the variable
        /// </summary>
        public string VariableName { get;  }
    }
}

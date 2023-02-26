using System.Collections.Generic;
using System.Diagnostics;

namespace TELL
{
    /// <summary>
    /// A variable in a TELL rule.
    /// These are the objects stored in a Rule or Predicate when an argument is a variable.
    /// They are also used as keys Substitutions, which map Vars to their run-time values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{DebugName}")]
    public class Var<T> : Term<T>, IVariable
    {
        /// <summary>
        /// Make a new variable
        /// </summary>
        /// <param name="name">Human-readable name of the variable</param>
        public Var(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Name of the variable, for debugging purposes
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Serial number for the next variable we create
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        public static int SerialNumberCounter;

        /// <summary>
        /// Unique number attached to this variable so if there are two variables with the same name,
        /// you can tell them apart in the debugger.  This is added to the end of the name when it's
        /// printed so you can tell which variable you're seeing.
        /// </summary>
        public int SerialNumber = SerialNumberCounter++;

        /// <summary>
        /// Yes, this is a variable
        /// </summary>
        public override bool IsVariable => true;
        
        /// <summary>
        /// Slightly less cumbersome way of making a variable
        /// </summary>
        /// <param name="s"></param>
        public static explicit operator Var<T>(string s) => new Var<T>(s);

        /// <summary>
        /// Make a new variable with exactly the same name
        /// </summary>
        public override Term Clone() => new Var<T>(Name);

        /// <inheritdoc />
        public override object? Instantiate(Dictionary<Term,Term>? vars) => (vars == null)?this:vars[this];

        /// <inheritdoc />
        public override string ToString() => Name+SerialNumber;

        private string DebugName => ToString();

        /// <summary>
        /// Used through the IVariable interface to get the name of the variable when you don't know its type
        /// </summary>
        public string VariableName => Name;
    }
}

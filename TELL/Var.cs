using System.Collections.Generic;
using System.Diagnostics;

namespace TELL
{
    [DebuggerDisplay("{DebugName}")]
    public class Var<T> : Term<T>
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

        public override bool IsVariable => true;
        
        public static explicit operator Var<T>(string s) => new Var<T>(s);

        /// <summary>
        /// Make a new variable with exactly the same name
        /// </summary>
        public override Term Clone() => new Var<T>(Name);

        public override object? Instantiate(Dictionary<Term,Term>? vars) => (vars == null)?this:vars[this];

        public override string ToString() => Name+SerialNumber;

        public string DebugName => ToString();
    }
}

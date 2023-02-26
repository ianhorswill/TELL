using System.Diagnostics;
using System.Text;

namespace TELL.Interpreter
{
    /// <summary>
    /// Represents a substitution of values for Variables.  It's basically just a dictionary.
    /// If the Variable's value is another Variable, then its "real" value is the second Variable's
    /// value, whatever that might be.  That the second variable might not have a value, in which
    /// case all we know is the two variables are equal.
    ///
    /// The substitution is represented as a linked list, so we can easily add new bindings to it
    /// without overwriting the original Substitution.  So a Substitution object contains one binding
    /// of one Variable, and a pointer to another Substitution (which could be null).
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebugName) + "}")]
    public class Substitution
    {
        /// <summary>
        /// Variable we're giving a value to
        /// </summary>
        public object Variable;
        /// <summary>
        /// Value we're giving to the variable
        /// </summary>
        public object? Value;
        /// <summary>
        /// The remaining bindings in the substitution
        /// </summary>
        public Substitution? Next;

        /// <summary>
        /// Extend a substitution with an additional variable binding
        /// </summary>
        /// <param name="variable">Variable to give a value to</param>
        /// <param name="value">Value to give to the variable</param>
        /// <param name="next">Substitution we're adding the binding to (possibly null)</param>
        public Substitution(object variable, object value, Substitution? next)
        {
            Variable = variable;
            Value = value;
            Next = next;
        }

        /// <summary>
        /// If the Substitution has a binding for the variable, then return true and output its value.
        /// Otherwise return false.
        /// </summary>
        /// <param name="substitution">Substitution to check</param>
        /// <param name="variable">Variable to find a value for</param>
        /// <param name="value">Output argument to write the value to</param>
        /// <returns>True if the variable has a value in this substitution</returns>
        public static bool Lookup(Substitution? substitution, Term variable, out object? value)
        {
            while (substitution != null)
            {
                if (ReferenceEquals(variable, substitution.Variable))
                {
                    value = substitution.Value;
                    return true;
                }

                substitution = substitution.Next;
            }

            value = null;
            return false;
        }

        private string DebugName => ToString();

        /// <inheritdoc />
        public override string ToString()
        {
            var b = new StringBuilder();
            var first = true;
            b.Append('[');
            for (var binding = this; binding != null; binding = binding.Next)
            {
                if (first)
                    first = false;
                else
                    b.Append(", ");

                b.Append($"{binding.Variable}->{binding.Value}");
            }

            b.Append(']');

            return b.ToString();
        }
    }
}

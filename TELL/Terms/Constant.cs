using System.Collections.Generic;
using System.Diagnostics;

namespace TELL
{
    /// <summary>
    /// Terms that represent constants of type T
    /// These always have the same value every time the code is run, as opposed to Vars, where they
    /// have to be looked up at runtime in the binding list.
    /// </summary>
    internal class Constant<T> : Term<T>
    {
        /// <summary>
        /// The actual constant
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// This is not a variable
        /// </summary>
        public override bool IsVariable => false;

        /// <summary>
        /// Make a Constant object to wrap the specified constant
        /// </summary>
        /// <param name="value">value to wrap</param>
        public Constant(T value)
        {
            // If value is a Term then something has gone wrong with your typing
            Debug.Assert(!(Value is Term));
            Value = value;
        }

        /// <summary>
        /// Called during Rule instantiation (see Rule.Instantiate).
        /// Just returns the value, unless the value is a goal, in which case we instantiate it.
        /// </summary>
        /// <param name="vars">Variable substitutions to perform.  Only relevant if T=AnyGoal</param>
        public override object? Instantiate(Dictionary<Term,Term>? vars) => (Value is Goal g)?(object)g.Instantiate(vars):Value;
        
        /// <inheritdoc />
        public override string ToString()
        {
            switch (Value)
            {
                case null: return "null";
                case string s: return $"\"{s}\"";
                default: return Value.ToString();
            }
        }
    }}

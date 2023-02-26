using System.Collections.Generic;
using System.Diagnostics;

namespace TELL
{
    /// <summary>
    /// A container for a set of predicates
    /// This is basically a namespace for predicates.
    /// It's presently only used by the REPL.  So if you aren't using that, you don't need a Program.
    /// </summary>
    [DebuggerDisplay("TELL.Program {Name}")]
    public class Program
    {
        /// <summary>
        /// The top of the stack is the program to which new predicates should be added.
        /// It's a stack out of paranoia that something bad will happen if multiple programs are loaded and they get mixed.
        /// </summary>
        private static readonly Stack<Program> LoadingPrograms = new Stack<Program>();

        /// <summary>
        /// Predicates and their names
        /// </summary>
        private readonly Dictionary<string, Predicate> predicates = new Dictionary<string, Predicate>();

        /// <summary>
        /// Name of the program, for debugging
        /// </summary>
        public readonly string Name;

        private Repl.Repl? _repl;

        /// <summary>
        /// Return a REPL associated with this program, creating it if necessary.
        /// </summary>
        public Repl.Repl Repl
        {
            get
            {
                if (_repl == null)
                    _repl = new Repl.Repl(this);
                return _repl;
            }
        }


        /// <summary>
        /// Make a new Program.  A Program is a container for predicates that are to be used with a Repl.
        /// To use: make a Program. call Program.Begin(), make a bunch of predicates, then call Program.End().
        /// Then you can access those predicates from the Repl
        /// </summary>
        /// <param name="name">Name of the program, for debugging purposes</param>
        public Program(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Start defining predicates for the program.  All predicates created between calling and calling End() will be added
        /// to the program.
        /// </summary>
        public void Begin() => LoadingPrograms.Push(this);

        /// <summary>
        /// Stop adding new predicates to this Program.
        /// </summary>
        public void End() => LoadingPrograms.Pop();

        /// <summary>
        /// The predicate with the specified name
        /// </summary>
        public Predicate this[string name] => predicates[name];

        /// <summary>
        /// The predicate with the specified name, or null if none is defined.
        /// </summary>
        public Predicate? PredicateNamed(string name) => predicates.TryGetValue(name, out var p) ? p : null;

        internal static void MaybeAddPredicate(Predicate p)
        {
            if (LoadingPrograms.Count>0)
                LoadingPrograms.Peek().Add(p);
        }

        private void Add(Predicate predicate)
        {
            predicates[predicate.Name] = predicate;
        }
    }
}

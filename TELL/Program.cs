using System.Collections.Generic;

namespace TELL
{
    public class Program
    {
        public static Program? Current;

        private readonly Dictionary<string, Predicate> predicates = new Dictionary<string, Predicate>();
        public readonly string Name;

        public Program(string name)
        {
            Name = name;
        }

        public void Begin() => Current = this;
        public void End() => Current = null;

        public Predicate this[string name] => predicates[name];

        public Predicate? PredicateNamed(string name) => predicates.TryGetValue(name, out var p) ? p : null;

        public static void MaybeAddPredicate(Predicate p)
        {
            if (Program.Current != null)
                Current.Add(p);
        }

        private void Add(Predicate predicate)
        {
            predicates[predicate.Name] = predicate;
        }
    }
}

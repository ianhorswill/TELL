using System.Collections.Generic;

namespace TELL
{
    public class Program
    {
        public static Stack<Program> LoadingPrograms = new Stack<Program>();

        private readonly Dictionary<string, Predicate> predicates = new Dictionary<string, Predicate>();
        public readonly string Name;

        public Program(string name)
        {
            Name = name;
        }

        public void Begin() => LoadingPrograms.Push(this);
        public void End() => LoadingPrograms.Pop();

        public Predicate this[string name] => predicates[name];

        public Predicate? PredicateNamed(string name) => predicates.TryGetValue(name, out var p) ? p : null;

        public static void MaybeAddPredicate(Predicate p)
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

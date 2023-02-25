using System;
using System.Collections.Generic;
using System.Linq;

namespace TELL.Repl
{
    public class Repl
    {
        public readonly Program Program;
        private readonly Parser parser;
        public Func<string, Term> ResolveConstant;
        public int MaxSolutions = 1000;

        private static Term NullConstantResolver(string s) =>
            throw new Exception($"Can't resolve ${s} because no resolver has been defined for $constants.");

        public Repl(Program program, Func<string, Term>? resolveConstant = null)
        {
            Program = program;
            ResolveConstant = resolveConstant??NullConstantResolver;
            parser = new Parser(this);
        }

        public IEnumerable<object?[]> Solutions(string goalString)
        {
            List<Goal> body = null!;
            var symbolTable = new Parser.SymbolTable();
            if (!parser.Body(new ParserState(goalString), symbolTable, (s, b) =>
                {
                    if (!s.End)
                        throw new Exception("Characters remaining after end of goal");
                    body = b;
                    return true;
                }))
                throw new Exception("Syntax error");
            var vars = symbolTable.VariablesInOrder.ToArray();
            var goal = new InstantiatedGoal(null!, vars);
            var rule = new InstantiatedRule(goal, body.Select(g => g.Instantiate(null)).ToArray());
            var results = new List<object?[]>();
            Prover.ProveUsingRule(rule.Head, rule, null, b =>
            {
                results.Add(vars.Select(v => Unifier.Dereference(v, b)).ToArray());
                return results.Count >= MaxSolutions;
            });
            return results;
        }
    }
}

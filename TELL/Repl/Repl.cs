using System;
using System.Collections.Generic;

namespace TELL.Repl
{
    public class Repl
    {
        public readonly Program Program;
        private readonly Parser parser;
        public Func<string, Term> ResolveConstant;

        private static Term NullConstantResolver(string s) =>
            throw new Exception($"Can't resolve ${s} because no resolver has been defined for $constants.");

        public Repl(Program program, Func<string, Term>? resolveConstant = null)
        {
            Program = program;
            ResolveConstant = resolveConstant??NullConstantResolver;
            parser = new Parser(this);
        }

        public IEnumerable<object> Solutions(string goal)
        {
            Goal parsedGoal = null!;
            if (!parser.Goal(new ParserState(goal), new Parser.SymbolTable(), (s, g) =>
                {
                    if (!s.End)
                        throw new Exception("Characters remaining after end of goal");
                    parsedGoal = g;
                    return true;
                }))
                throw new Exception("Syntax error");
            return parsedGoal.SolutionsUntyped;
        }
    }
}

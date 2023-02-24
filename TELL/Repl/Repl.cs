using System;
using System.Collections.Generic;

namespace TELL.Repl
{
    public class Repl
    {
        private readonly Parser parser;

        public Repl(Program program)
        {
            parser = new Parser(program);
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

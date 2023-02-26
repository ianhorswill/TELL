using System;
using System.Collections.Generic;
using System.Linq;
using TELL.Interpreter;

namespace TELL.Repl
{
    /// <summary>
    /// Support for Read/Evaluate/Print/Loops (REPLs), i.e. command line interpreters
    /// This converts strings containing code to executable queries and runs them, returning the results
    /// </summary>
    public class Repl
    {
        /// <summary>
        /// The collection of predicates to allow the user to call
        /// </summary>
        public readonly Program Program;

        /// <summary>
        /// Parser object to use to parse queries
        /// </summary>
        private readonly Parser parser;

        /// <summary>
        /// Called when the user types a term of the form $string or $"string".  Maps string to a Term.
        /// Fill this in to handle whatever application-specific term syntax you need.
        /// </summary>
        public Func<string, Term> ResolveConstant;

        /// <summary>
        /// Maximum number of solutions to return to a query
        /// </summary>
        public int MaxSolutions = 1000;

        /// <summary>
        /// Default constant resolver to use
        /// </summary>
        private static Term NullConstantResolver(string s) =>
            throw new Exception($"Can't resolve ${s} because no resolver has been defined for $constants.");

        /// <summary>
        /// Make a new Repl.
        /// </summary>
        /// <param name="program">Collection of predicates to be callable form the repl</param>
        /// <param name="resolveConstant">Constant resolver to use for $string terms, or null if $string shouldn't be supported</param>
        public Repl(Program program, Func<string, Term>? resolveConstant = null)
        {
            Program = program;
            ResolveConstant = resolveConstant??NullConstantResolver;
            parser = new Parser(this);
        }

        /// <summary>
        /// Find all solutions (up to MaxSolutions) to query and return them as a sequence of arrays,
        /// each containing the values of each variable appearing in the query, in left-to-right order.
        /// </summary>
        /// <param name="goalString">String containing the query</param>
        /// <returns>List of solutions</returns>
        /// <exception cref="Exception">If code throws and exception or there is a syntax error</exception>
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
            // ReSharper disable once CoVariantArrayConversion
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

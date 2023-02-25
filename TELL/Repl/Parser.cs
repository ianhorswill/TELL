﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TELL.Repl.ParserState;

namespace TELL.Repl
{
    internal class Parser
    {
        public readonly Repl Repl;

        public Parser(Repl repl)
        {
            Repl = repl;
        }

        public Predicate? PredicateNamed(string name) => Repl.Program.PredicateNamed(name);
        public static bool Identifier(ParserState s, Continuation<string> k) => s.ReadToken(char.IsLetter, k);
        public bool Predicate(ParserState s, Continuation<Predicate> k) => s.ReadToken(char.IsLetter, PredicateNamed, k);

        public static bool Number(ParserState s, Continuation<Term> k)
            => s.ReadToken(char.IsDigit, (s, digits) => k(s, new Constant<int>(int.Parse(digits))));

        public static bool String(ParserState s, Continuation<Term> k)
            => s.Match("\"",
                s2 => s2.ReadToken(c => c != '"',
                    (s3, str) => s3.Match("\"",
                        s4 => k(s4, new Constant<string>(str)))));

        public static bool Variable(ParserState s, Continuation<Term> k) 
            => s.ReadToken(char.IsLetter, str => (Var<object>) str, k);

        public bool Term(ParserState s, Continuation<Term> k)
            => Number(s, k) || String(s, k) || Variable(s, k) || ExternalConstant(s,k);

        private bool ExternalConstant(ParserState s, Continuation<Term> k)
        {
            return s.Match("$",
                s2 => s2.ReadToken(char.IsLetter, (s3, str) => k(s3, Repl.ResolveConstant(str)))
                      || s2.Match("\"",
                          s3 => s3.ReadToken(c => c != '"',
                              (s4, str) => s4.Match("\"",
                                  s5 => k(s5, Repl.ResolveConstant(str))))));
        }

        public bool Goal(ParserState s, SymbolTable vars, Continuation<Goal> k)
            => Predicate(s,
                (s1, predicate) => 
                  predicate != null 
                  && s1.Match("[",
                    s2 => s2.DelimitedList<Term>(Term, ",",
                        (s3, args) => s3.Match("]",
                            s4 => k(s4, MakeGoal(predicate, args, vars))))));

        public Goal MakeGoal(Predicate p, List<Term> args, SymbolTable vars)
        {
            var defaults = p.DefaultVariables;
            if (defaults.Length != args.Count)
                throw new ArgumentException(
                    $"Predicate {p.Name} expects {defaults.Length} arguments, but was passed {args.Count}");
            var finalArgs = new Term[defaults.Length];
            for (var i = 0; i < defaults.Length; i++)
            {
                if (args[i] is IVariable v)
                    finalArgs[i] = vars.GetVariable(v.VariableName, (Term)defaults[i]);
                else
                    finalArgs[i] = args[i];
            }

            return p.GetGoal(finalArgs);
        }

        public class SymbolTable
        {
            private readonly Dictionary<string, Term> VariableTable = new Dictionary<string, Term>();

            /// <summary>
            /// Get the variable with this name, if one has already been made.  Otherwise, make one of the same type as defaultArg.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="defaultArg"></param>
            /// <returns></returns>
            public Term GetVariable(string name, Term defaultArg)
            {
                if (!VariableTable.TryGetValue(name, out var variable))
                {
                    variable = defaultArg.MakeVariable(name);
                    VariableTable[name] = variable;
                }
                return variable;
            }
        }
    }
}
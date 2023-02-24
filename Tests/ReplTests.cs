using System.Net.Mail;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TELL;
using TELL.Repl;
using static TELL.Language;

namespace Tests
{
    [TestClass]
    public class ReplTests
    {
        [TestMethod]
        public void Eval()
        {
            var prog = new Program("EvalTest");
            prog.Begin();
            var s = (Var<string>)"s";
            var Foo = Predicate("Foo", s);
            Foo["a"].Fact();
            Foo["b"].Fact();
            prog.End();
            CollectionAssert.AreEqual(new object[] { "a", "b" },
                new Repl(prog).Solutions("Foo[s]").ToArray());
        }

        [TestMethod]
        public void Externals()
        {
            var prog = new Program("EvalTest");
            prog.Begin();
            var s = (Var<string>)"s";
            var Foo = Predicate("Foo", s);
            Foo["a"].Fact();
            Foo["b"].Fact();
            prog.End();
            CollectionAssert.AreEqual(new object[] { "a" },
                new Repl(prog, _ =>new Constant<string>("a")).Solutions("Foo[$\"s\"]").ToArray());
        }
    }
}
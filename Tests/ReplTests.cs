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
                new Repl(prog).Solutions("Foo[s]").Select(a => a[0]).ToArray());
        }

        [TestMethod]
        public void DualGoal()
        {
            var prog = new Program("EvalTest");
            prog.Begin();
            var s = (Var<string>)"s";
            var Foo = Predicate("Foo", s);
            Foo["a"].Fact();
            Foo["b"].Fact();
            Foo["d"].Fact();
            var Bar = Predicate("Bar", s);
            Bar["b"].Fact();
            Bar["c"].Fact();
            Bar["d"].Fact();
            prog.End();
            CollectionAssert.AreEqual(new object[] { "b", "d" },
                new Repl(prog).Solutions("Foo[s], Bar[s]").Select(a => a[0]).ToArray());
        }

        [TestMethod]
        public void RuleCall()
        {
            var prog = new Program("EvalTest");
            prog.Begin();
            var s = (Var<string>)"s";
            var Foo = Predicate("Foo", s);
            Foo["a"].Fact();
            Foo["b"].Fact();
            Foo["d"].Fact();
            var Bar = Predicate("Bar", s);
            Bar["b"].Fact();
            Bar["c"].Fact();
            Bar["d"].Fact();
            var Baz = Predicate("Baz", s).If(Foo[s], Bar[s]);
            prog.End();
            CollectionAssert.AreEqual(new object[] { "b", "d" },
                new Repl(prog).Solutions("Baz[s]").Select(a => a[0]).ToArray());
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
            Assert.AreEqual(1, new Repl(prog, _ =>new Constant<string>("a")).Solutions("Foo[$\"s\"]").Count());
        }
    }
}
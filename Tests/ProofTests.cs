using Microsoft.VisualStudio.TestTools.UnitTesting;
using TELL;
using static TELL.Primitives;

namespace Tests
{
    [TestClass]
    public class ProofTests
    {
        public void AssertTrue(AnyGoal g) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(g.IsTrue);
        public void AssertNot(AnyGoal g) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(g.IsTrue);

        /// <summary>
        /// This lets you test ProveUsingRule directly, at least a little, so you don't have to have
        /// all the Prove and ProveUsingRule both work before you can test other of them.
        ///
        /// IMPORTANT: This will only test whether ProveUsingRule matches the head of the rule, since
        /// we don't try any rules with bodies (subgoals), since they would require having all the other
        /// Prove methods working too.  But this will at least help you start the debugging process.
        /// </summary>
        [TestMethod]
        public void RuleTest()
        {
            var p = new TellPredicate<string>("p");
            // This is a rule that just says "p[a] is true unconditionally"
            var factRule = new Rule(p["a"]);
            
            // You should be able to prove p["a"] from that
            Assert.IsTrue(Prover.ProveUsingRule(p["a"].Instantiate(null), factRule.Instantiate(), null, _ => true));
            
            // You should not be able to prove p["b"] from that
            Assert.IsFalse(Prover.ProveUsingRule(p["b"].Instantiate(null), factRule.Instantiate(), null, _ => true));

            var v = new Var<string>("?v");

            // You should be able to prove p[variable] from that and get back that the variable's value is "a".
            Assert.IsTrue(Prover.ProveUsingRule(p[v].Instantiate(null), factRule.Instantiate(), null, s => Unifier.Dereference(v, s).Equals("a")));

            // This says p is true for any argument
            var variableRule = new Rule(p[v]);

            // You should be able to prove p["a"] from that
            Assert.IsTrue(Prover.ProveUsingRule(p["a"].Instantiate(null), variableRule.Instantiate(), null, _ => true));

            // You should also be able to prove p["b"] from that
            Assert.IsTrue(Prover.ProveUsingRule(p["b"].Instantiate(null), variableRule.Instantiate(), null, _ => true));
        }
        
        /// <summary>
        /// This tests whether both Prove and ProveUsingRule work, although still doesn't involve subgoals
        ///
        /// This is the equivalent of the Step code:
        /// [predicate]
        /// P a.
        /// P b.
        ///
        /// And then we run the queries: [P a], [P b], [P c], and [P ?v]
        /// </summary>
        [TestMethod]
        public void FactTest()
        {
            // Make a predicate p
            var p = new TellPredicate<string>("p");

            // assert that p(a) and p(b) are true and nothing else
            p["a"].Fact();
            p["b"].Fact();

            // Make sure we can prove p(a) and p(b)
            AssertTrue(p["a"]);
            AssertTrue(p["b"]);

            // Make sure we can't prove p(c)
            AssertNot(p["c"]);

            // Make sure we can prove p(?v)
            var v = new Var<string>("?v");
            AssertTrue(p[v]);

            // Make sure we can prove p(?v) and it gives us ?v = "a"
            Assert.AreEqual("a", p[v].SolveFor(v));

            // Make sure p(?v) has exactly two solutions and they're ?v=a and ?v=b.
            var solutions = p[v].SolveForAll(v);
            Assert.AreEqual(2, solutions.Count);
            Assert.AreEqual("a", solutions[0]);
            Assert.AreEqual("b", solutions[1]);
        }

        /// <summary>
        /// This tests whether rules with subgoals (bodies) work, but
        /// doesn't test whether recursive rules work.
        ///
        /// This is the equivalent of the Step code:
        /// [predicate]
        /// P a.
        /// P b.
        ///
        /// [predicate]
        /// Q ?x: [P ?x]
        ///
        /// And then we find all the solutions to Q.  Since Q is true just when P is true, we should the the solutions a and b.
        /// </summary>
        [TestMethod]
        public void SubgoalTest()
        {
            // Make a predicate p
            var p = new TellPredicate<string>("p");

            // assert that p(a) and p(b) are true and nothing else
            p["a"].Fact();
            p["b"].Fact();

            var q = new TellPredicate<string>("q");
            var x = (Var<string>)"x";
            q[x].If(p[x]);

            var v = new Var<string>("v");
            var solutions = q[v].SolveForAll(v);
            Assert.AreEqual(2, solutions.Count);
            Assert.AreEqual("a", solutions[0]);
            Assert.AreEqual("b", solutions[1]);
        }

        /// <summary>
        /// This tests whether rules with multiple subgoals work.
        /// but still doesn't test recursive rules.
        ///
        /// This is the equivalent off the Step code:
        /// [predicate]
        /// P a.
        /// P b.
        ///
        /// [predicate]
        /// R c.
        /// R b.
        ///
        /// [predicate]
        /// Q ?x: [P ?x] [R ?x]
        /// </summary>
        [TestMethod]
        public void MultipleSubgoalTest()
        {
            // Make a predicate p
            var p = new TellPredicate<string>("p");

            // assert that p(a) and p(b) are true and nothing else
            p["a"].Fact();
            p["b"].Fact();

            // Make a predicate q
            var q = new TellPredicate<string>("q");

            // assert that p(a) and p(b) are true and nothing else
            q["c"].Fact();
            q["b"].Fact();

            var r = new TellPredicate<string>("r");
            var x = new Var<string>("x");
            r[x].If(p[x], q[x]);

            var v = new Var<string>("v");
            var solutions = r[v].SolveForAll(v);
            Assert.AreEqual(1, solutions.Count);
            Assert.AreEqual("b", solutions[0]);
        }

        /// <summary>
        /// This tests whether recursive rules work
        /// If this works, then probably everything is correct.
        ///
        /// It tests whether you can use the prover to test whether
        /// paths exist in a directed graph.
        ///
        /// The graph is defined by the "edge" predicate, and the existence
        /// of a directed path is tested by the "reachable" predicate,
        /// whose second rule calls reachable recursively.
        ///
        /// This is the equivalent of the Step code:
        /// [predicate]
        /// Edge a b.
        /// Edge b c.
        /// Edge c d.
        /// Edge x y.
        ///
        /// [predicate[
        /// Reachable ?v ?v.
        /// Reachable ?start ?end: [Edge ?start ?neighbor] [Reachable ?neighbor ?end]
        /// </summary>
        [TestMethod]
        public void DirectedAcyclicGraphTest()
        {
            // edge[x,y] means there's an edge from x to y in the graph
            var edge = new TellPredicate<string, string>("edge");
            // Add edges a->b, b->c, c->d
            edge["a", "b"].Fact();
            edge["b", "c"].Fact();
            edge["c", "d"].Fact();
            edge["b", "m"].Fact();
            edge["m", "n"].Fact();
            // Add edge x->y.  Notice that x and y are unconnected to a,b,c,d
            edge["x", "y"].Fact();

            // reachable[x,y] means there's a path from x to y in the graph
            var reachable = new TellPredicate<string, string>("connected");
            var v = (Var<string>)"v";
            // An edge if reachable from itself
            reachable[v, v].Fact();
            // end if reachable from start if it's reachable from one of start's neighbors
            var start = (Var<string>)"end";
            var end = (Var<string>)"end";
            var neighbor = (Var<string>)"neighbor";
            reachable[start, end].If(edge[start, neighbor], reachable[neighbor, end]);

            // Should be able to prove d is reachable from a
            AssertTrue(reachable["a", "d"]);
            // But not vice-versa
            AssertNot(reachable["d", "a"]);
            // Should be able to prove n is reachable from a
            AssertTrue(reachable["a", "n"]);
            // But not vice-versa
            AssertNot(reachable["n", "a"]);
            // Should not be able to prove a is connected to x.
            AssertNot(reachable["a", "x"]);
        }
    }
    
}
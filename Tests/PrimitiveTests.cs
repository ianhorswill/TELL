using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TELL;
using TELL.Interpreter;
using static TELL.Language;

namespace Tests
{
    [TestClass]
    public class PrimitiveTests
    {
        public void AssertTrue(Goal g) => Assert.IsTrue(g.IsTrue);
        public void AssertNot(Goal g) => Assert.IsFalse(g.IsTrue);


        [TestMethod]
        public void NotTest()
        {
            var p = new TELL.Predicate<string>("p");
            p["a"].Fact();
            AssertTrue(Not[p["b"]]);
            AssertNot(Not[p["a"]]);
        }

        [TestMethod]
        public void OnceTest()
        {
            var p = new TELL.Predicate<string>("p");
            p["a"].Fact();
            p["c"].Fact();
            p["d"].Fact();
            var s = (Var<string>)"s";
            var onceResult = Once[p[s]].SolveForAll(s).ToArray();
            CollectionAssert.AreEqual(new[] {"a"},
                onceResult);
            AssertNot(Once[p["b"]]);
        }

        [TestMethod]
        public void BoundTest()
        {
            var v = (Var<string>)"v";
            var p = new TELL.Predicate<string>("p");
            p["a"].Fact();

            AssertTrue(Bound<int>(1));
            AssertTrue(And[p[v], Bound(v)]);
            AssertNot(Bound(v));

            AssertNot(Unbound<int>(1));
            AssertNot(And[p[v], Unbound(v)]);
            AssertTrue(Unbound(v));
        }

        [TestMethod]
        public void AndTest()
        {
            var x = (Var<string>)"x";
            var y = (Var<string>)"y";
            var z = (Var<string>)"z";
            AssertTrue(And[Same(x,y), Same(y,z), Same("a", x), Same("a", z)]);
            AssertNot(And[Same(x,y), Same(y,z), Same("a", x), Same("b", z)]);
        }

        [TestMethod]
        public void SameTest()
        {
            AssertTrue(Same<string>("a", "a"));
            AssertNot(Same<string>("a", "b"));
            var same3 = new Predicate<string, string, string>("same3");
            var x = (Var<string>)"x";
            var y = (Var<string>)"y";
            var z = (Var<string>)"z";
            AssertTrue(Same(x,x));
            AssertTrue(Same(x,y));
            AssertTrue(Same(x,"foo"));
            AssertTrue(Same("foo", x));
            same3[x,y,z].If(Same(x,y), Same(y, z));
            Assert.AreEqual("a", same3["a", y, z].SolveFor(z));
            var sameTest = new TELL.Predicate<string>("sameTest");
            sameTest[x].If(Same(x,y), Same(y,"foo"));
            AssertTrue(sameTest["foo"]);
            AssertNot(sameTest["bar"]);
        }

        [TestMethod]
        public void DifferentTest()
        {
            AssertNot(Different<string>("a", "a"));
            AssertTrue(Different<string>("a", "b"));
            var x = (Var<string>)"x";
            var y = (Var<string>)"y";
            AssertNot(Different(x,x));
            AssertNot(Different(x,y));
            AssertNot(Different(x,"foo"));
            AssertNot(Different("foo",y));
        }

        [TestMethod]
        public void MemberTest()
        {
            var strings = new [] { "a", "b", "c"};
            AssertTrue(Member<string>("b", strings));
            AssertNot(Member<string>("d", strings));
            var elt = (Var<string>)"elt";
            var copiedList = Member(elt, strings).SolveForAll(elt);
            Assert.AreEqual(3, copiedList.Count);
            for (var i = 0; i < strings.Length; i++)
                Assert.AreEqual(strings[i], copiedList[i]);
        }

        public void FunctionTest()
        {
            var f = Predicate("inc", (int n) => n+1);
            var n = (Var<int>)"n";
            Assert.AreEqual(2, f[1, n].SolveFor(n));
            AssertTrue(f[1,2]);
        }

        /// <summary>
        /// This is an example of interop to some non-trivial C# subsystem, in this case, System.Reflection.
        /// </summary>
        [TestMethod]
        public void ReflectionTest()
        {
            // Special primitives

            // True when argument is a type defined in the TELL assembly.
            var type = new TELL.Predicate<Type>("type", ModeDispatch<Type>("type",
                // ReSharper disable once ConvertTypeCheckToNullCheck
                x => x is Type,
                () => Assembly.GetAssembly(typeof(Term))!.DefinedTypes));
            // True when that type has that field
            var field = new Predicate<Type,FieldInfo>("field",
                ModeDispatch("field",
                    (t, f) => t.GetFields().Contains(f),
                    t => t.GetFields(),
                    t => new [] { t.DeclaringType! },
                    () => Assembly.GetAssembly(typeof(Term))!.DefinedTypes.SelectMany(t => t.GetFields().Select(f => ((Type)t,f)))));
            // True when the name of the field is the string
            var fieldName = Predicate("fieldName", (FieldInfo f) => f.Name);
            // Type of the field
            var fieldType = Predicate("fieldType", (FieldInfo f) => f.FieldType);

            // Predicate that finds types with a field named "Predicate"
            var hasPredicateField = new TELL.Predicate<Type>("hasPredicateField");
            var t = (Var<Type>)"t";
            var f = (Var<FieldInfo>)"f";
            hasPredicateField[t].If(type[t], field[t, f], fieldName[f, "Predicate"]);

            // Find types that contain fields named Predicate
            AssertTrue(hasPredicateField[typeof(Goal)]);
            var typesWithPredicateField = hasPredicateField[t].SolveForAll(t);
            Assert.IsTrue(typesWithPredicateField.Count > 6);
            Assert.IsTrue(typesWithPredicateField.Contains(typeof(Goal)));
            Assert.IsTrue(typesWithPredicateField.Contains(typeof(InstantiatedGoal)));

            // Find types that contain fields of type AnyPredicate
            var predicateContainers = And[type[t], field[t, f], fieldType[f, typeof(Predicate)]].SolveForAll(t);
            Assert.IsTrue(predicateContainers.Contains(typeof(Goal)));
            Assert.IsFalse(predicateContainers.Contains(typeof(Term)));

            // Find types that contain fields of their own type
            var selfReferentialTypes = And[type[t], field[t, f], fieldType[f, t]].SolveForAll(t);
            Assert.IsTrue(selfReferentialTypes.Contains(typeof(Substitution)));
            Assert.IsFalse(selfReferentialTypes.Contains(typeof(Language)));
        }
    }
}
using System;
using TELL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static TELL.Unifier;

namespace Tests
{
    [TestClass]
    public class UnificationTests
    {
        [TestMethod]
        public void DereferenceTest()
        {
            var v1 = new Var<string>("?v1");
            var v2 = new Var<string>("?v2");
            // "a" is just "a"
            Assert.AreEqual("a", Dereference("a", null));
            
            // An unbound variable is just itself
            Assert.AreEqual(v1, Dereference(v1, null));
            
            // If a variable is bound to a constant, then dereferencing the variable returns the constant
            Assert.AreEqual("a", Dereference(v1, new Substitution(v1, "a", null)));
            
            // If a variable is bound to an unbound variable, the dereferencing the first variable returns the second
            Assert.AreEqual(v2, Dereference(v1, new Substitution(v1, v2, null)));
            
            // If v1 is bound to v2 and v2 is bound to a constant, then dereferencing v1 returns the constant
            Assert.AreEqual("a", Dereference(v1, new Substitution(v1, v2, new Substitution(v2, "a", null))));
        }

        [TestMethod]
        public void UnifyConstantConstant()
        {
            // Unifying different constants should fail
            Assert.IsFalse(Unify("a", "b", null, out var result));
            
            // Unifying identical constants should succeed and shouldn't make a new substitution
            Assert.IsTrue(Unify("a", "a", null, out result) && result == null);
        }

        [TestMethod]
        public void UnifyVariableConstant()
        {
            var v = new Var<string>("?v");
            
            // Unifying an unbound variable with a constant should give the variable that constant as a value
            Assert.IsTrue(Unify(v, "a", null, out var result)
                          && Substitution.Lookup(result, v, out var value)
                          && value.Equals("a"));
            
            // Unifying a variable tha already has that same constant as a value should work
            Assert.IsTrue(Unify(v, "a", new Substitution(v, "a", null), out result));
            
            // But it should fail if the variable's existing value is different from the value we're unifying to
            Assert.IsFalse(Unify(v, "a", new Substitution(v, "b", null), out result));
        }

        [TestMethod]
        public void UnifyConstantVariable()
        {
            var v = new Var<string>("?v");
            
            // Unifying an unbound variable with a constant should give the variable that constant as a value
            Assert.IsTrue(Unify("a", v, null, out var result)
                          && Substitution.Lookup(result, v, out var value)
                          && value.Equals("a"));
            
            // Unifying a variable tha already has that same constant as a value should work
            Assert.IsTrue(Unify("a", v, new Substitution(v, "a", null), out result));
            
            // But it should fail if the variable's existing value is different from the value we're unifying to
            Assert.IsFalse(Unify("a", v, new Substitution(v, "b", null), out result));
        }

        [TestMethod]
        public void UnifyVariableVariable()
        {
            var v1 = new Var<string>("?v1");
            var v2 = new Var<string>("?v2");
            
            // Unifying a variable with itself should not add another binding to the substitution
            Assert.IsTrue(Unify(v1, v1, null, out var subst) && subst == null);
            
            // Unifying two unbound variables binds them together
            Assert.IsTrue(Unify(v1, v2, null, out subst) && VariablesAliased(v1, v2, subst));
            
            // Unifying v1 to a v2 that has a value gives v1 the same value
            Assert.IsTrue(Unify(v1, v2, new Substitution(v2, "a", null), out subst)
                            && Substitution.Lookup(subst, v1, out var value)
                            && value.Equals("a"));
            
            // Unifying v2 to a v1 that has a value gives v2 the same value
            Assert.IsTrue(Unify(v1, v2, new Substitution(v1, "a", null), out subst)
                          && Substitution.Lookup(subst, v2, out  value)
                          && value.Equals("a"));
            
            // Unifying two variable that have already been unified to different values should fail
            Assert.IsFalse(Unify(v1, v2, new Substitution(v1, "a", new Substitution(v2, "b", null)), out subst));
        }

        /// <summary>
        /// True if v1 is substituted with v1 or vice-versa
        /// </summary>
        private bool VariablesAliased<T>(Var<T> v1, Var<T> v2, Substitution s)
        {
            return (Substitution.Lookup(s, v1, out var value) && value == v2)
                   || (Substitution.Lookup(s, v2, out value) && value == v1);
        }

        [TestMethod]
        public void UnifyArray()
        {
            // Can't unify arrays with different numbers of elements
            Assert.IsFalse(UnifyArrays(new object[1], new object[2], null, out var subst));
            
            var v1 = new Var<string>("?v1");
            var v2 = new Var<string>("?v2");
            
            // Unifying [v1, v2] with [v2, "a"] should unify v1 with v2, then v2 with "a"
            // so they both have the value "a"
            Assert.IsTrue(UnifyArrays(new object[] { v1, v2 },
                                        new object[] { v2, "a" },
                                        null,
                                        out subst)
                            && Dereference(v1, subst).Equals("a")
                            && Dereference(v2, subst).Equals("a"));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zadanie_0;

namespace Test_Zadanie_0
{
    [TestClass]
    public class Test_Calculator
    {
        [TestMethod]
        public void Test_Add()
        {
            Calculator calculator = new Calculator();
            Assert.AreEqual(calculator.Add(3, 2), 5);
        }
    }
}
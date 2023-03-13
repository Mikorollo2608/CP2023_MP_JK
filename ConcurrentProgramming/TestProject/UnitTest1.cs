using ClassLibrary;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Calculator calc = new Calculator();

            Assert.IsNotNull(calc);
            Assert.AreEqual(5, calc.sub(7, 2));
        }
    }
}
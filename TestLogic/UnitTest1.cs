using Logic;

namespace TestLogic
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Ball ball = new Ball(5, 8);

            Assert.IsNotNull(ball);
            Assert.AreEqual(5, ball.getX());
            Assert.AreEqual(8, ball.getY());

        }
    }
}
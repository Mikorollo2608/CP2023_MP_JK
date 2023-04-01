using Logic;

namespace TestLogic
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestGetters()
        {
            Ball ball = new Ball(5, 8, 1, 2);

            Assert.IsNotNull(ball);
            Assert.AreEqual(5, ball.getX());
            Assert.AreEqual(8, ball.getY());
            Assert.AreEqual(1, ball.getVelocityX());
            Assert.AreEqual(2, ball.getVelocityY());

            Assert.AreEqual(5, Ball.radius);
        }

        [TestMethod]
        public void TestSetters()
        {
            Ball ball = new Ball(1, 1, 5, 6);

            Assert.AreEqual(5, ball.getVelocityX());
            Assert.AreEqual(6, ball.getVelocityY());

            ball.setVelocityX(10);
            ball.setVelocityY(13);

            Assert.AreEqual(10, ball.getVelocityX());
            Assert.AreEqual(13, ball.getVelocityY());
        }

        [TestMethod]
        public void TestMove()
        {
            Ball ball = new Ball(12, 15, 3, 2);
            int borderX = 100;
            int borderY = 80;

            Assert.AreEqual(12, ball.getX());
            Assert.AreEqual(15, ball.getY());
            
            //Test normal move
            ball.move(borderX, borderY);
            Assert.AreEqual(15, ball.getX());
            Assert.AreEqual(17, ball.getY());

            //Test move outside one border
            ball.setVelocityX(100);
            ball.move(borderX, borderY);
            Assert.AreEqual(95, ball.getX());
            Assert.AreEqual(-100, ball.getVelocityX());
            Assert.AreEqual(19, ball.getY());
        }
    }
}
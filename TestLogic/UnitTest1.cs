using Logic;

namespace TestLogic
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBallMove()
        {
            Ball ball = new Ball(0, 0, null);
            int x = ball.X;
            int y = ball.Y;
            Thread.Sleep(150);
            Assert.AreNotEqual(x, ball.X);
            Assert.AreNotEqual(y, ball.Y);
        }

        public void TestSimulationBoardKeepBallInbound()
        {
            LogicAbstractApi board = LogicAbstractApi.CreateLogicApi(5,100,100);
            board.CreateBall(0, 0);
            Thread.Sleep(150);
            int x = board.GetX(0);
            int y = board.GetY(0);
            Assert.IsTrue(x >= 5, "X is " + x);
            Assert.IsTrue(y >= 5, "Y is " + x);
        }
    }
}
using Logic;
using System.ComponentModel;

namespace TestLogic
{
    [TestClass]
    public class LogicApiTest
    {

        public void Foo(object obj, PropertyChangedEventArgs e) { Console.WriteLine("Do nothing"); }

        [TestMethod]
        public void TestSimulationBoardKeepBallInbound()
        {
            LogicAbstractApi board = LogicAbstractApi.CreateLogicApi(5,100,100);
            board.CreateBall(50, 50);
            Thread.Sleep(150);
            int x = board.GetX(0);
            int y = board.GetY(0);
            Assert.IsTrue(x >= 5, "X is " + x);
            Assert.IsTrue(y >= 5, "Y is " + x);
        }

        [TestMethod]
        public void TestBallMove()
        {
            Ball ball = new Ball(0, 0, Foo);
            int x = ball.X;
            int y = ball.Y;
            Thread.Sleep(150);
            Assert.AreNotEqual(x, ball.X);
            Assert.AreNotEqual(y, ball.Y);
        }
    }
}
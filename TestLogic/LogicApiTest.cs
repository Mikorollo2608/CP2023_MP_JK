using Data;
using Logic;
using System.Numerics;

namespace TestLogic
{
    internal class TestBall : BallApi
    {
        private double x, y;
        private BallEvent? BallPublisher;
        public TestBall(int BallRadius, double X, double Y, double XVel, double YVel)
        {
            Radius = BallRadius;
            x = X;
            y = Y;
            XVelocity = XVel;
            YVelocity = YVel;
        }
        public override void Start()
        {
            //Do nothing
        }
        public override void Stop()
        {
            //Do nothing
        }
        public override int Radius { get; }
        public override double X { get { return x; } }
        public override double Y { get { return y; } }
        public override double XVelocity { get; set; }
        public override double YVelocity { get; set; }

        public void Move()
        {
            x += XVelocity;
            y += YVelocity;
            BallPublisher?.Invoke(this);
        }

        public override void AddSubscriber(BallEvent sub)
        {
            BallPublisher += sub;
        }
    }

    internal class TestMovementBox : MovementBoxApi
    {
        public override int Height { get; }
        public override int Width { get; }

        public TestMovementBox(int Height, int Width)
        {
            this.Width = Width;
            this.Height = Height;
        }
    }

    internal class TestLogger : LoggerApi
    {
        public TestLogger() { }

        public override void addToQueue(DateTime time, int ball1, int ball2)
        {
        }
    }

    [TestClass]
    public class LogicApiTest
    {

        internal void BallChangedBall(BallApi ball) { }
        internal void BallChangedLogic(int index) { }

        [TestMethod]
        public void TestSimulationBoardKeepBallInbound()
        {
            TestBall b1 = new TestBall(10, 15, 50, -3, 0);
            TestBall b2 = new TestBall(10, 85, 50, 3, 0);
            TestBall b3 = new TestBall(10, 50, 15, 0, -3);
            TestBall b4 = new TestBall(10, 50, 85, 0, 3);
            LogicAbstractApi board = LogicAbstractApi.CreateLogicApi(new TestMovementBox(100, 100), 10, null, null);
            board.CreateBall(b1);
            board.CreateBall(b2);
            board.CreateBall(b3);
            board.CreateBall(b4);

            b1.Move();
            Assert.AreEqual(-3, b1.XVelocity);
            Assert.AreEqual(0, b1.YVelocity);
            b1.Move();
            Assert.AreEqual(3, b1.XVelocity);
            Assert.AreEqual(0, b1.YVelocity);

            b2.Move();
            Assert.AreEqual(3, b2.XVelocity);
            Assert.AreEqual(0, b2.YVelocity);
            b2.Move();
            Assert.AreEqual(-3, b2.XVelocity);
            Assert.AreEqual(0, b2.YVelocity);

            b3.Move();
            Assert.AreEqual(0, b3.XVelocity);
            Assert.AreEqual(-3, b3.YVelocity);
            b3.Move();
            Assert.AreEqual(0, b3.XVelocity);
            Assert.AreEqual(3, b3.YVelocity);

            b4.Move();
            Assert.AreEqual(0, b4.XVelocity);
            Assert.AreEqual(3, b4.YVelocity);
            b4.Move();
            Assert.AreEqual(0, b4.XVelocity);
            Assert.AreEqual(-3, b4.YVelocity);
        }
        [TestMethod]
        public void TestSimulationBoardCollisions()
        {
            TestBall b1 = new TestBall(10, 30, 50, 3, 0);
            TestBall b2 = new TestBall(10, 51, 50, -3, 0);
            LogicAbstractApi board = LogicAbstractApi.CreateLogicApi(new TestMovementBox(100, 100), 10, null, new TestLogger());
            board.CreateBall(b1);
            board.CreateBall(b2);

            Vector2 ball1Vel = new Vector2((float)b1.XVelocity, (float)b1.YVelocity);
            Vector2 ball2Vel = new Vector2((float)b2.XVelocity, (float)b2.YVelocity);
            Vector2 ball1Pos = new Vector2((float)(b1.X + b1.XVelocity), (float)(b1.Y + b1.YVelocity));
            Vector2 ball2Pos = new Vector2((float)b2.X, (float)b2.Y);
            Vector2 newBall1Vel = Vector2.Subtract(ball1Vel, Vector2.Multiply(Vector2.Subtract(ball1Pos, ball2Pos), (float)(Vector2.Dot(ball1Vel - ball2Vel, ball1Pos - ball2Pos) / Math.Pow(Vector2.Distance(ball1Pos, ball2Pos), 2))));
            Vector2 newBall2Vel = Vector2.Subtract(ball2Vel, Vector2.Multiply(Vector2.Subtract(ball2Pos, ball1Pos), (float)(Vector2.Dot(ball2Vel - ball1Vel, ball2Pos - ball1Pos) / Math.Pow(Vector2.Distance(ball2Pos, ball1Pos), 2))));

            b1.Move();
            Assert.AreEqual(newBall1Vel.X, b1.XVelocity);
            Assert.AreEqual(newBall1Vel.Y, b1.YVelocity);
            Assert.AreEqual(newBall2Vel.X, b2.XVelocity);
            Assert.AreEqual(newBall2Vel.Y, b2.YVelocity);
        }
    }
}
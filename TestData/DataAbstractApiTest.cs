using Data;

namespace TestData
{
    [TestClass]
    public class DataAbstractApiTest
    {
        [TestMethod]
        public void PlaneTest()
        {
            int width = 100;
            int height = 150;
            MovementBoxApi plane = MovementBoxApi.CreateBox(width, height);
            Assert.IsNotNull(plane);
            Assert.AreEqual(width, plane.Width);
            Assert.AreEqual(height, plane.Height);
        }

        [TestMethod]
        public void BallTest()
        {
            BallEvent foo = (BallApi ball) => { return; };
            int BallRadius = 12;
            int x = 120;
            int y = 230;
            double XVel = 3.5;
            double YVel = -4;
            BallApi ball = BallApi.CreateNewBall(BallRadius, x, y, XVel, YVel, foo, false);
            Assert.AreEqual(BallRadius, ball.Radius);
            Assert.AreEqual(x, ball.X);
            Assert.AreEqual(y, ball.Y);
            Assert.AreEqual(XVel, ball.XVelocity);
            Assert.AreEqual(YVel, ball.YVelocity);
        }

        [TestMethod]
        public async Task BallMovingTest()
        {
            BallEvent foo = (BallApi ball) => { return; };
            int BallRadius = 12;
            int x = 120;
            int y = 230;
            double XVel = 3.5;
            double YVel = -4;
            BallApi ball = BallApi.CreateNewBall(BallRadius, x, y, XVel, YVel, foo, false);
            Assert.AreEqual(BallRadius, ball.Radius);
            Assert.AreEqual(x, ball.X);
            Assert.AreEqual(y, ball.Y);
            Assert.AreEqual(XVel, ball.XVelocity);
            Assert.AreEqual(YVel, ball.YVelocity);
            ball.Start();
            await Task.Delay(20);
            ball.Stop();
            Assert.IsTrue(ball.X > x);
            Assert.IsTrue(ball.Y < y);
        }
    }
}
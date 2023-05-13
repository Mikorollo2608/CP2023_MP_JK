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
            BallApi plane = BallApi.CreateNewBall(BallRadius, x, y, XVel, YVel, foo, false);
        }
    }
}
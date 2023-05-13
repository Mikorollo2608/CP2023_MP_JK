using Data;
using System.Numerics;

namespace Logic
{
    internal class SimulationBoard : LogicAbstractApi
    {
        public override int BallRadius { get; }
        public override event BallPositionEvent BallMoved;

        private Random rand = new Random();
        private bool IsSimulationRunning = false;
        private List<BallApi> Balls = new List<BallApi>();
        private BallEvent TriggerCollisions;
        private MovementBox Box;
        private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();

        public SimulationBoard(int Radius, int Width, int Height, BallPositionEvent Subscriber)
        {
            Box = MovementBox.CreateBox(Width, Height);
            BallRadius = Radius;
            BallMoved += Subscriber;
            TriggerCollisions += BallsColisions;
        }

        public override void CreateBall(int x, int y)
        {

            double XVelocity = (rand.NextDouble() * 14) - 7;
            double YVelocity = (rand.NextDouble() * 14) - 7;
            try
            {
                lockSlim.EnterWriteLock();
                Balls.Add(BallApi.CreateNewBall(x, y, XVelocity, YVelocity, KeepBallInbound, IsSimulationRunning));
            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        public override double GetX(int BallNumber)
        {
            return Balls[BallNumber].GetX();
        }
        public override double GetY(int BallNumber)
        {
            return Balls[BallNumber].GetY();
        }
        public override int GetBallCount()
        {
            return Balls.Count;
        }

        public override void Start()
        {
            IsSimulationRunning = true;
            foreach (BallApi ball in Balls)
            {
                ball.Start();
            }
        }

        public override void Stop()
        {
            IsSimulationRunning = false;
            foreach (BallApi ball in Balls)
            {
                ball.Stop();
            }
        }

        private void OnBallMoved(BallApi ball)
        {
            TriggerCollisions.Invoke(ball);
            BallMoved?.Invoke(Balls.FindIndex(a => a == ball));
        }

        private void BallsColisions(BallApi ball)
        {
            //int ballIndex;
            //int ballCount;
            //int index;
            //lock (Balls)
            //{
            //    Balls.Sort(CompareBalls);
            //    ballIndex = Balls.IndexOf(ball);
            //    ballCount= Balls.Count;
            //    index = ballIndex-1;
            //    ball.Stop();
            //    while(index >= 0 && Math.Abs(Balls[index].GetX() - ball.GetX())<2*BallRadius) {
            //        Balls[index].Stop();
            //        if (CalculateBallsDistance(ball, Balls[index]) < 2 * BallRadius)
            //        {
            //            //TODO change velocity
            //            Velocities vel = CalculateNewVelocities(ball, Balls[index]);
            //            ball.XVelocity = vel.Ball1X;
            //            ball.YVelocity = vel.Ball1Y;
            //            Balls[index].XVelocity = vel.Ball2X;
            //            Balls[index].YVelocity = vel.Ball2Y;
            //            ball.Start();
            //            Balls[index].Start();
            //            return;
            //        }
            //        Balls[index].Start();
            //        index--;
            //    }
            //    index = ballIndex + 1;
            //    while (index < ballCount && Math.Abs(Balls[index].GetX() - ball.GetX()) < 2 * BallRadius)
            //    {
            //        if (CalculateBallsDistance(ball, Balls[index]) < 2 * BallRadius)
            //        {
            //            //TODO change velocity
            //            Velocities vel = CalculateNewVelocities(ball, Balls[index]);
            //            ball.XVelocity = vel.Ball1X;
            //            ball.YVelocity = vel.Ball1Y;
            //            Balls[index].XVelocity = vel.Ball2X;
            //            Balls[index].YVelocity = vel.Ball2Y;
            //            ball.Start();
            //            Balls[index].Start();
            //            return;
            //        }
            //        Balls[index].Start();
            //        index++;
            //    }
            //}
            ball.Stop();
            lockSlim.EnterReadLock();
            try
            {
                foreach (BallApi b in Balls)
                {
                    b.Stop();
                    if (b != ball)
                    {
                        if (CalculateBallsDistance(ball, b) < 2 * BallRadius)
                        {
                            Velocities vel = CalculateNewVelocities(ball, b);
                            ball.XVelocity = vel.Ball1X;
                            ball.YVelocity = vel.Ball1Y;
                            b.XVelocity = vel.Ball2X;
                            b.YVelocity = vel.Ball2Y;
                        }
                    }
                    b.Start();
                }
                ball.Start();

            }
            finally
            {
                lockSlim.ExitReadLock();
            }
        }

        private void KeepBallInbound(BallApi ball)
        {
            if (ball.GetX() - BallRadius < 0 || ball.GetX() + BallRadius > Box.Width) { ball.XVelocity = -ball.XVelocity; }
            if (ball.GetY() - BallRadius < 0 || ball.GetY() + BallRadius > Box.Height) { ball.YVelocity = -ball.YVelocity; }
            OnBallMoved(ball);
        }

        private static int CompareBalls(BallApi ball1, BallApi ball2)
        {
            ball1.Stop();
            ball2.Stop();
            if (ball1.GetX() - ball2.GetX() == 0)
            {
                ball1.Start();
                ball2.Start();
                return Math.Sign(ball1.GetY() - ball2.GetY());
            }
            ball1.Start();
            ball2.Start();
            return Math.Sign(ball1.GetX() - ball2.GetX());
        }

        private static double CalculateBallsDistance(BallApi ball1, BallApi ball2)
        {
            return Math.Sqrt(Math.Pow(ball1.GetX() - ball2.GetX(), 2) + Math.Pow(ball1.GetY() - ball2.GetY(), 2));
        }

        private Velocities CalculateNewVelocities(BallApi ball1, BallApi ball2)
        {
            Velocities ret = new Velocities();
            Vector2 ball1Vel = new Vector2((float)ball1.XVelocity, (float)ball1.YVelocity);
            Vector2 ball2Vel = new Vector2((float)ball2.XVelocity, (float)ball2.YVelocity);
            Vector2 ball1Pos = new Vector2((float)ball1.GetX(), (float)ball1.GetY());
            Vector2 ball2Pos = new Vector2((float)ball2.GetX(), (float)ball2.GetY());
            var a = Vector2.Multiply(Vector2.Subtract(ball1Pos, ball2Pos), (float)(Vector2.Dot(ball1Vel - ball2Vel, ball1Pos - ball2Pos) / Math.Pow(Vector2.Distance(ball1Pos, ball2Pos), 2)));
            var b = Vector2.Subtract(ball1Pos, ball2Pos);
            var c = (float)(Vector2.Dot(ball1Vel - ball2Vel, ball1Pos - ball2Pos) / Math.Pow(Vector2.Distance(ball1Pos, ball2Pos), 2));
            Vector2 newBall1Vel = Vector2.Subtract(ball1Vel, Vector2.Multiply(Vector2.Subtract(ball1Pos, ball2Pos), (float)(Vector2.Dot(ball1Vel - ball2Vel, ball1Pos - ball2Pos) / Math.Pow(Vector2.Distance(ball1Pos, ball2Pos), 2))));
            Vector2 newBall2Vel = Vector2.Subtract(ball2Vel, Vector2.Multiply(Vector2.Subtract(ball2Pos, ball1Pos), (float)(Vector2.Dot(ball2Vel - ball1Vel, ball2Pos - ball1Pos) / Math.Pow(Vector2.Distance(ball2Pos, ball1Pos), 2))));
            ret.Ball1X = (double)newBall1Vel.X;
            ret.Ball1Y = (double)newBall1Vel.Y;
            ret.Ball2X = (double)newBall2Vel.X;
            ret.Ball2Y = (double)newBall2Vel.Y;
            return ret;
        }
    }
}

internal struct Velocities
{
    public double Ball1X { get; set; }
    public double Ball2X { get; set; }
    public double Ball1Y { get; set; }
    public double Ball2Y { get; set; }
}
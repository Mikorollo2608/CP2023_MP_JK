using Data;
using Logic;
using System;
using System.Diagnostics;
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
        private CollisionCache cache;


        public SimulationBoard(int Radius, int Width, int Height, BallPositionEvent Subscriber)
        {
            Box = MovementBox.CreateBox(Width, Height);
            BallRadius = Radius;
            BallMoved += Subscriber;
            TriggerCollisions += BallsColisions;
            cache = new CollisionCache(CheckForCollision);
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
            ball.Stop();
            lockSlim.EnterWriteLock();
            try
            {
                foreach (BallApi b in Balls)
                {
                    b.Stop();
                    if (b != ball)
                    {
                        if (CalculateBallsDistanceNextFrame(ball, b) < 2 * BallRadius)
                        {
                            if (!cache.Contains(ball, b))
                            {
                                cache.Add(ball, b);
                                Velocities vel = CalculateNewVelocitiesNextFrame(ball, b);
                                ball.XVelocity = vel.Ball1X;
                                ball.YVelocity = vel.Ball1Y;
                                b.XVelocity = vel.Ball2X;
                                b.YVelocity = vel.Ball2Y;
                                b.Start();
                                ball.Start();
                                break;
                            }
                        }
                    }
                    b.Start();
                }
                ball.Start();

            }
            finally
            {
                lockSlim.ExitWriteLock();
            }
        }

        private void KeepBallInbound(BallApi ball)
        {
            if (ball.GetX() - BallRadius < 0 && Math.Sign(ball.XVelocity) == -1) ball.XVelocity = -ball.XVelocity;
            else if (ball.GetX() + BallRadius > Box.Width && Math.Sign(ball.XVelocity) == 1) { ball.XVelocity = -ball.XVelocity; }
            if (ball.GetY() - BallRadius < 0 && Math.Sign(ball.YVelocity) == -1) { ball.YVelocity = -ball.YVelocity; }
            else if (ball.GetY() + BallRadius > Box.Height && Math.Sign(ball.YVelocity) == 1) { ball.YVelocity = -ball.YVelocity; }
            OnBallMoved(ball);
        }

        private bool CheckForCollision(BallApi ball1, BallApi ball2)
        {
            ball1.Stop();
            ball2.Stop();
            bool ret = CalculateBallsDistance(ball1, ball2) < 2 * BallRadius;
            ball1.Start();
            ball1.Start();
            return ret;
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

        internal static double CalculateBallsDistance(BallApi ball1, BallApi ball2)
        {
            return Math.Sqrt(Math.Pow(ball1.GetX() - ball2.GetX(), 2) + Math.Pow(ball1.GetY() - ball2.GetY(), 2));
        }

        internal static double CalculateBallsDistanceNextFrame(BallApi ball1, BallApi ball2)
        {
            return Math.Sqrt(Math.Pow(ball1.GetX() + ball1.XVelocity - ball2.GetX(), 2) + Math.Pow(ball1.GetY() + ball1.YVelocity - ball2.GetY(), 2));
        }

        private Velocities CalculateNewVelocities(BallApi ball1, BallApi ball2)
        {
            Velocities ret = new Velocities();
            Vector2 ball1Vel = new Vector2((float)ball1.XVelocity, (float)ball1.YVelocity);
            Vector2 ball2Vel = new Vector2((float)ball2.XVelocity, (float)ball2.YVelocity);
            Vector2 ball1Pos = new Vector2((float)ball1.GetX(), (float)ball1.GetY());
            Vector2 ball2Pos = new Vector2((float)ball2.GetX(), (float)ball2.GetY());



            //TODO usunac



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

        private Velocities CalculateNewVelocitiesNextFrame(BallApi ball1, BallApi ball2)
        {
            Velocities ret = new Velocities();
            Vector2 ball1Vel = new Vector2((float)ball1.XVelocity, (float)ball1.YVelocity);
            Vector2 ball2Vel = new Vector2((float)ball2.XVelocity, (float)ball2.YVelocity);
            Vector2 ball1Pos = new Vector2((float)(ball1.GetX() + ball1.XVelocity), (float)(ball1.GetY() + ball1.YVelocity));
            Vector2 ball2Pos = new Vector2((float)ball2.GetX(), (float)ball2.GetY());



            //TODO usunac



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

internal delegate bool CollisionDetection(BallApi ball1, BallApi ball2);

internal class CollisionCache
{
    private List<CollisionPair> cache = new List<CollisionPair>();
    private ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim();
    private CollisionDetection AreBallsConnected;

    public CollisionCache(CollisionDetection foo)
    {
        AreBallsConnected = foo;
    }

    public void Add(BallApi ball1, BallApi ball2)
    {
        CollisionPair pair = new CollisionPair(ball1, ball2);
        try
        {
            lockSlim.EnterWriteLock();
            cache.Add(pair);
            Task.Run(() => { ClearCache(pair); });
        }
        finally { lockSlim.ExitWriteLock(); }
    }

    public bool Contains(BallApi ball1, BallApi ball2)
    {
        try
        {
            lockSlim.EnterReadLock();
            foreach (CollisionPair collisionPair in cache)
            {
                if (collisionPair.Contains(ball1, ball2)) { return true; }
            }
            return false;
        }
        finally { lockSlim.ExitReadLock(); }
    }

    private async void ClearCache()
    {
        while (true)
        {
            try
            {
                lockSlim.EnterWriteLock();
                if (cache.Count != 0) cache.RemoveAt(0);
            }
            finally
            {
                lockSlim.ExitWriteLock();
                await Task.Delay(25);
            }
        }
    }

    private async void ClearCache(CollisionPair pair)
    {
        while (AreBallsConnected(pair.ball1, pair.ball2))
        {
            await Task.Delay(20);
        }
        try
        {
            lockSlim.EnterWriteLock();
            cache.Remove(pair);
        }
        finally
        {
            lockSlim.ExitWriteLock();
        }
    }
}

internal struct CollisionPair
{
    public BallApi ball1 { get; set; }
    public BallApi ball2 { get; set; }

    public CollisionPair(BallApi ball1, BallApi ball2)
    {
        this.ball1 = ball1;
        this.ball2 = ball2;
    }

    public bool Contains(BallApi ball1, BallApi ball2)
    {
        return (Object.ReferenceEquals(this.ball1, ball2) && Object.ReferenceEquals(this.ball2, ball1)) || (Object.ReferenceEquals(this.ball1, ball1) && Object.ReferenceEquals(this.ball2, ball2));
    }
}

internal struct Velocities
{
    public double Ball1X { get; set; }
    public double Ball2X { get; set; }
    public double Ball1Y { get; set; }
    public double Ball2Y { get; set; }
}
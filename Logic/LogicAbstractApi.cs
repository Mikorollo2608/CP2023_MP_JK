using Data;

namespace Logic
{
    public delegate void BallPositionEvent(int index);
    public abstract class LogicAbstractApi
    {
        public abstract int BallRadius { get; }

        public abstract event BallPositionEvent BallMoved;
        public abstract void CreateBall(int x, int y);
        public abstract double GetX(int BallNumber);
        public abstract double GetY(int BallNumber);
        public abstract int GetBallCount();

        public abstract void Start();

        public abstract void Stop();

        public static LogicAbstractApi CreateLogicApi(int BallRadius, int BoardWidth, int BoardHeight, BallPositionEvent Subscriber)
        {
            return new SimulationBoard(BallRadius, BoardWidth, BoardHeight, Subscriber);
        }
    }

    internal class SimulationBoard : LogicAbstractApi
    {
        private Random rand = new Random();
        private bool IsSimulationRunning = false;
        private List<BallApi> Balls = new List<BallApi>();
        public override int BallRadius { get; }

        public override event BallPositionEvent BallMoved;

        private MovementBox Box;

        public SimulationBoard(int Radius, int Width, int Height, BallPositionEvent Subscriber)
        {
            Box = MovementBox.CreateBox(Width, Height);
            BallRadius = Radius;
            BallMoved += Subscriber;
        }

        public override void CreateBall(int x, int y)
        {
            double XVelocity = (rand.NextDouble() * 14) - 7;
            double YVelocity = (rand.NextDouble() * 14) - 7;
            Balls.Add(BallApi.CreateNewBall(x, y, XVelocity, YVelocity, KeepBallInbound, IsSimulationRunning));
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

        protected void OnBallMoved(int index)
        {
            BallMoved?.Invoke(index);
        }

        private void KeepBallInbound(BallApi ball)
        {
            if (ball.GetX() - BallRadius < 0 || ball.GetX() + BallRadius > Box.Width) { ball.XVelocity = -ball.XVelocity; }
            if (ball.GetY() - BallRadius < 0 || ball.GetY() + BallRadius > Box.Height) { ball.YVelocity = -ball.YVelocity; }
            OnBallMoved(Balls.FindIndex(a => a == ball));
        }
    }
}

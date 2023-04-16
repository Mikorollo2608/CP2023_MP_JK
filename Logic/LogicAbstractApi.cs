using Data;

namespace Logic
{
    public delegate void BallPositionEvent(int index);
    public abstract class LogicAbstractApi
    {
        public abstract int BallRadius { get; }
        public abstract int BoardWidth { get; }
        public abstract int BoardHeight { get; }
        public abstract event BallPositionEvent BallMoved;
        public abstract void CreateBall(int x, int y);
        public abstract int GetX(int BallNumber);
        public abstract int GetY(int BallNumber);
        public abstract int GetBallCount();

        public static LogicAbstractApi CreateLogicApi(int BallRadius, int BoardWidth, int BoardHeight, BallPositionEvent Subscriber)
        {
            return new SimulationBoard(BallRadius, BoardWidth, BoardHeight, Subscriber);
        }
    }

    internal class SimulationBoard : LogicAbstractApi
    {

        private List<Ball> Balls = new List<Ball>();
        public override int BallRadius { get; }
        public override int BoardWidth { get; }
        public override int BoardHeight { get; }

        public override event BallPositionEvent BallMoved;

        private DataAbstractApi DataApi = DataAbstractApi.CreateApi();

        public SimulationBoard(int Radius, int Width, int Height, BallPositionEvent Subscriber)
        {
            BallRadius = Radius;
            BoardWidth = Width;
            BoardHeight = Height;
            BallMoved += Subscriber;
        }

        public override void CreateBall(int x, int y)
        {
            Balls.Add(new Ball(x, y, KeepBallInbound));
        }

        public override int GetX(int BallNumber)
        {
            return Balls[BallNumber].X;
        }
        public override int GetY(int BallNumber)
        {
            return Balls[BallNumber].Y;
        }
        public override int GetBallCount()
        {
            return Balls.Count;
        }

        protected void OnBallMoved(int index)
        {
            BallMoved?.Invoke(index);
        }

        private void KeepBallInbound(Ball ball)
        {
            if (ball.X - BallRadius < 0) { ball.X = 0 + BallRadius; }
            else if (ball.X + BallRadius > BoardWidth) { ball.X = BoardWidth - BallRadius; }
            if (ball.Y - BallRadius < 0) { ball.Y = 0 + BallRadius; }
            else if (ball.Y + BallRadius > BoardHeight) { ball.Y = BoardHeight - BallRadius; }
            OnBallMoved(Balls.FindIndex(a => a == ball));

            Console.WriteLine("keep " + ball.X + " " +ball.Y);
        }
    }
}

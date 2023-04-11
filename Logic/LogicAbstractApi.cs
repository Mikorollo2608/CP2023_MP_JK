namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public abstract int BallRadius { get; }
        public abstract int BoardWidth { get; }
        public abstract int BoardHeight { get; }
        public abstract void CreateBall(int x, int y);
        public abstract int GetX(int BallNumber);
        public abstract int GetY(int BallNumber);
        public abstract int GetBallCount();

        public static LogicAbstractApi CreateLogicApi(int BallRadius, int BoardWidth, int BoardHeight)
        {
            return new SimulationBoard(BallRadius, BoardWidth, BoardHeight);
        }
    }

    internal class SimulationBoard : LogicAbstractApi
    {

        private List<Ball> Balls = new List<Ball>();
        public override int BallRadius { get; }
        public override int BoardWidth { get; }
        public override int BoardHeight { get; }

        public SimulationBoard(int Radius, int Width, int Height)
        {
            BallRadius = Radius;
            BoardWidth = Width;
            BoardHeight = Height;
        }

        public override void CreateBall(int x, int y)
        {
            Balls.Add(new Ball(x, y));
        }

        public override int GetX(int BallNumber)
        {
            return Balls[BallNumber].GetX();
        }
        public override int GetY(int BallNumber)
        {
            return Balls[BallNumber].GetY();
        }
        public override int GetBallCount()
        {
            return Balls.Count;
        }
    }
}

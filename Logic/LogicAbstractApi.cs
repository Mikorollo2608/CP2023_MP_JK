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

}

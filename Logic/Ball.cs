using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic
{
    internal delegate void BallEvent(Ball ball);

    internal class Ball : IDisposable
    {
        private Random random = new Random();
        public int X { get; set; }
        public int Y { get; set; }

        private Timer BallTimer;
        private event BallEvent BallMoved;

        public Ball(int x, int y, BallEvent function)
        {
            X = x;
            Y = y;
            BallTimer = new Timer(Move, null, 0, 100);
            BallMoved += function;
        }

        private void Move(object? state)
        {
            X += random.Next(21) - 10;
            Y += random.Next(21) - 10;
            OnBallMoved();
        }

        protected void OnBallMoved()
        {
            BallMoved?.Invoke(this);
        }

        public BallEvent GetPublisher() { return BallMoved; }

        public void Dispose() { BallTimer.Dispose(); }
    }
}
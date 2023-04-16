using System.Threading;
using System.Timers;

namespace Logic
{
    internal delegate void BallEvent(Ball ball);

    internal class Ball : IDisposable
    {
        private Random random = new Random();
        public int X { get; set; }
        public int Y { get; set; }

        private System.Timers.Timer BallTimer;
        private event BallEvent BallMoved;

        public Ball(int x, int y, bool IsSimulationRunning, BallEvent function)
        {
            X = x;
            Y = y;
            BallTimer = new System.Timers.Timer(100);
            BallTimer.Elapsed += Move;
            BallTimer.AutoReset = true;
            BallTimer.Enabled = IsSimulationRunning;
            BallMoved += function;
        }

        public void Start()
        {
            BallTimer.Enabled = true;
        }

        public void Stop()
        {
            BallTimer.Enabled = false;
        }

        private void Move(object? source, ElapsedEventArgs e)
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
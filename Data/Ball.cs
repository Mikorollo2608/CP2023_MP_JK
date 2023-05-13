using System.Timers;

namespace Data
{
    internal class Ball : BallApi, IDisposable
    {
        private BallEvent? BallPublisher;
        private System.Timers.Timer? BallTimer;

        private double x;
        private double y;
        public override double XVelocity { get; set; }
        public override double YVelocity { get; set; }

        public override double GetX() { return x; }
        public override double GetY() { return y; }


        public Ball(double x, double y, double XVelovity, double YVelovity, BallEvent subscriber, bool IsSimulationRunning)
        {
            this.x = x;
            this.y = y;
            this.XVelocity = XVelovity;
            this.YVelocity = YVelovity;
            this.BallPublisher = subscriber;
            BallTimer = new System.Timers.Timer(30);
            BallTimer.Elapsed += Move;
            BallTimer.AutoReset = true;
            BallTimer.Enabled = IsSimulationRunning;
        }

        private void Move(object? source, ElapsedEventArgs e)
        {
            x += XVelocity;
            y += YVelocity;
            OnBallMoved();
        }

        protected void OnBallMoved()
        {
            BallPublisher?.Invoke(this);
        }

        public override void Start()
        {
            BallTimer.Enabled = true;
        }

        public override void Stop()
        {
            BallTimer.Enabled = false;
        }

        public void Dispose() { BallTimer.Dispose(); }
    }
}

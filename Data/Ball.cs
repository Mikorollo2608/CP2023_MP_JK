using System.Diagnostics;

namespace Data
{
    internal class Ball : BallApi
    {
        private BallEvent? BallPublisher;
        private bool IsSimulationRunning;
        private double x;
        private double y;

        public override int Radius { get; }
        public override double XVelocity { get; set; }
        public override double YVelocity { get; set; }

        public override double X { get { return x; } }
        public override double Y { get { return y; } }

        public Ball(int BallRadius, double X, double Y, double XVelocity, double YVelocity, BallEvent subscriber, bool IsSimulationRunning)
        {
            Radius = BallRadius;
            this.x = X;
            this.y = Y;
            this.XVelocity = XVelocity;
            this.YVelocity = YVelocity;
            BallPublisher = subscriber;
            this.IsSimulationRunning = IsSimulationRunning;
            Task.Run(() => { Move(); });
        }

        private async void Move()
        {
            while (true)
            {
                if (IsSimulationRunning)
                {
                    x += XVelocity;
                    y += YVelocity;
                    OnBallMoved();
                    await Task.Delay(20);
                }
                else
                {
                    await Task.Delay(10);
                }
            }
        }

        protected void OnBallMoved()
        {
            BallPublisher?.Invoke(this);
        }

        public override void Start()
        {
            IsSimulationRunning = true;
        }

        public override void Stop()
        {
            IsSimulationRunning = false;
        }
    }
}

using System.Diagnostics;

namespace Data
{
    internal class Ball : BallApi
    {
        private BallEvent? BallPublisher;
        private bool IsSimulationRunning;
        private double x;
        private double y;
        private double xVel;
        private double yVel;
        private double velocityLength;

        public override int Radius { get; }
        public override double XVelocity { get { return xVel; } 
            set { 
                xVel = value;
                velocityLength = Math.Sqrt(Math.Pow(xVel, 2) + Math.Pow(yVel, 2));
            }
        }
        public override double YVelocity {
            get { return yVel; }
            set
            {
                yVel = value;
                velocityLength = Math.Sqrt(Math.Pow(xVel, 2) + Math.Pow(yVel, 2));
            }
        }

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
                    await Task.Delay(12+(int)Math.Floor(50/ (velocityLength*2)));
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

        public override void AddSubscriber(BallEvent sub)
        {
            BallPublisher += sub;
        }
    }
}

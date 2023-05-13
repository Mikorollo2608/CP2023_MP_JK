namespace Data
{
    public delegate void BallEvent(BallApi ball);

    public abstract class BallApi
    {
        public abstract double XVelocity { get; set; }
        public abstract double YVelocity { get; set; }
        public abstract double GetX();
        public abstract double GetY();
        public abstract void Start();
        public abstract void Stop();

        public static BallApi CreateNewBall(double x, double y, double XVelocity, double YVelocity, BallEvent subscriber, bool IsSimulationRunning)
        {
            return new Ball(x, y, XVelocity, YVelocity, subscriber, IsSimulationRunning);
        }

    }

    public abstract class MovementBoxApi
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public static MovementBoxApi CreateBox(int width, int height)
        {
            return new Plane(width, height);
        }
    }

    internal class Plane : MovementBoxApi
    {
        public override int Width { get; }
        public override int Height { get; }

        public Plane(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }
    }

}
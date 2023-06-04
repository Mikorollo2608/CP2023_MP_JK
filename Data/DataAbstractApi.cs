namespace Data
{
    public delegate void BallEvent(BallApi ball);

    public abstract class BallApi
    {
        public abstract int Radius { get; }
        public abstract double XVelocity { get; set; }
        public abstract double YVelocity { get; set; }

        public abstract double X { get; }
        public abstract double Y { get; }
        public abstract void Start();
        public abstract void Stop();

        public abstract void AddSubscriber(BallEvent sub);

        public static BallApi CreateNewBall(int BallRadius, double x, double y, double XVelocity, double YVelocity, BallEvent subscriber, bool IsSimulationRunning)
        {
            return new Ball(BallRadius, x, y, XVelocity, YVelocity, subscriber, IsSimulationRunning);
        }

    }

    public abstract class LoggerApi
    {
        public abstract void addToQueue(DateTime time, int ball1, int ball2);
        public static LoggerApi CreateLogger(String fileName)
        {
            return new Logger(fileName);
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
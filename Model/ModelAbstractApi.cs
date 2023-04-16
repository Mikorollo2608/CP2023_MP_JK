using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Logic;

namespace Model
{
    public abstract class ModelAbstractApi
    {
        public abstract int BallRadius { get; }
        public abstract int BoardWidth { get; }
        public abstract int BoardHeight { get; }
        public abstract Canvas Canvas { get; set; }
        public abstract List<Ellipse> ellipseCollection { get; }
        public abstract void CreateEllipses(int BallsNumber);
        public abstract void Start(int BallsNumber);
        public abstract void Stop();
        public abstract void MoveAll();

        public abstract void Move(int index);

        public static ModelAbstractApi CreateApi(int BallRadius, int BoardWidth, int BoardHeight)
        {
            return new PresentationModel(BallRadius, BoardWidth, BoardHeight);
        }
    }

    internal class PresentationModel : ModelAbstractApi
    {
        private LogicAbstractApi Simulation;
        private readonly int StartOffsetX = -20;
        private readonly int EndOffsetX = -4;
        private readonly int StartOffsetY = 161;
        private readonly int EndOffsetY = 177;

        public override int BallRadius { get; }
        public override int BoardWidth { get; }
        public override int BoardHeight { get; }
        public override Canvas Canvas { get; set; }
        public override List<Ellipse> ellipseCollection { get; }

        public PresentationModel(int BallRadius, int BoardWidth, int BoardHeight) 
        {
            this.BallRadius = BallRadius;
            this.BoardWidth = BoardWidth;
            this.BoardHeight = BoardHeight;
            Simulation = LogicAbstractApi.CreateLogicApi(this.BallRadius, this.BoardWidth + BallRadius, this.BoardHeight + BallRadius, Move);
            Canvas = new Canvas();
            Canvas.Width = this.BoardWidth;
            Canvas.Height = this.BoardHeight;
            Canvas.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.VerticalAlignment = VerticalAlignment.Top;
            ellipseCollection = new List<Ellipse>();
        }

        public override void CreateEllipses(int BallsNumber) 
        {
            Random random = new Random();
            for (int i = 0; i < BallsNumber; i++)
            {
                Simulation.CreateBall(random.Next(0, BoardWidth), random.Next(0, BoardHeight));
                Ellipse ellipse = new Ellipse();
                ellipse.Width = BallRadius;
                ellipse.Height = BallRadius;
                ellipse.Fill = Brushes.Red;
                Canvas.SetLeft(ellipse, correctPosition(Simulation.GetX(i), StartOffsetX + BallRadius, EndOffsetX));
                Canvas.SetTop(ellipse, correctPosition(Simulation.GetY(i), StartOffsetY + BallRadius, EndOffsetY));
                ellipseCollection.Add(ellipse);
                Canvas.Children.Add(ellipse);
            }
        }
        public override void Start(int BallsNumber) 
        {
            CreateEllipses(BallsNumber);
        }
        public override void Stop() 
        {
        }
        public override void MoveAll()
        {
            for (int i = 0; i < Simulation.GetBallCount(); i++)
            {
                Canvas.SetLeft(ellipseCollection[i], correctPosition(Simulation.GetX(i), StartOffsetX, EndOffsetX));
                Canvas.SetTop(ellipseCollection[i], correctPosition(Simulation.GetY(i), StartOffsetY, EndOffsetY));
            }

        }

        public override void Move(int index)
        {
            int left = correctPosition(Simulation.GetX(index), StartOffsetX, EndOffsetX);
            int right = correctPosition(Simulation.GetY(index), StartOffsetY, EndOffsetY);
            Application.Current.Dispatcher.Invoke(new Action (() => {
                Canvas.SetLeft(ellipseCollection[index], left);
                Canvas.SetTop(ellipseCollection[index], right);
            }));
        }


        private int correctPosition(int Value, int StartOffset, int EndOffset)
        {
            int newValue = Value + StartOffset;

            if (newValue + BallRadius > BoardWidth + EndOffset)
            {
                newValue = BoardWidth - BallRadius + EndOffset;
            }
            return newValue;
        }
    }
}
using System.ComponentModel;
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
        public abstract void Start();
        public abstract void Stop();
        public abstract void Move();

        public static ModelAbstractApi CreateApi(int BallRadius, int BoardWidth, int BoardHeight)
        {
            return new PresentationModel(BallRadius, BoardWidth, BoardHeight);
        }
    }

    internal class PresentationModel : ModelAbstractApi
    {
        private LogicAbstractApi simulation;

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
            simulation = LogicAbstractApi.CreateLogicApi(this.BallRadius, this.BoardWidth, this.BoardHeight);
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
                simulation.CreateBall(random.Next(100, 2 * (BoardWidth - BallRadius)), random.Next(100, 2 * (BoardWidth - BallRadius)));
                Ellipse ellipse = new Ellipse();
                ellipse.Width = BallRadius;
                ellipse.Height = BallRadius;
                ellipse.Fill = Brushes.Blue;
                //Canvas.SetLeft(ellipse, simulation.GetX(i));
                //Canvas.SetTop(ellipse, simulation.GetY(i));
                ellipseCollection.Add(ellipse);
            }
        }
        public override void Start() { }
        public override void Stop() { }
        public override void Move() { }
    }
}
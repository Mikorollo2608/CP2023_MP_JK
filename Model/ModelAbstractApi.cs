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
        public abstract Border Border{ get; set; }
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

        public override int BallRadius { get; }
        public override int BoardWidth { get; }
        public override int BoardHeight { get; }
        public Canvas Canvas { get; set; }

        private Random random = new Random();

        public override Border Border { get; set; }
        public override List<Ellipse> ellipseCollection { get; }

        public PresentationModel(int BallRadius, int BoardWidth, int BoardHeight) 
        {
            this.BallRadius = BallRadius;
            this.BoardWidth = BoardWidth;
            this.BoardHeight = BoardHeight;
            Simulation = LogicAbstractApi.CreateLogicApi(this.BallRadius, this.BoardWidth+BallRadius, this.BoardHeight + BallRadius, Move);
            Canvas = new Canvas();
            Border = new Border();
            Canvas.Width = this.BoardWidth;
            Canvas.Height = this.BoardHeight;
            Canvas.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.VerticalAlignment = VerticalAlignment.Center;
            Border.Width = this.BoardWidth+10;
            Border.Height = this.BoardHeight+10;
            Border.HorizontalAlignment = HorizontalAlignment.Center;
            Border.VerticalAlignment = VerticalAlignment.Center;
            Border.BorderThickness = new Thickness(5);
            Border.BorderBrush = Brushes.Green;
            Border.Child = Canvas;
            ellipseCollection = new List<Ellipse>();
        }

        public override void CreateEllipses(int BallsNumber) 
        {
            for (int i = 0; i < BallsNumber; i++)
            {
                Simulation.CreateBall(random.Next(0, BoardWidth), random.Next(0, BoardHeight));
                Ellipse ellipse = new Ellipse();
                ellipse.Width = BallRadius;
                ellipse.Height = BallRadius;
                ellipse.Fill = Brushes.Red;
                ellipse.HorizontalAlignment = HorizontalAlignment.Center;
                ellipse.VerticalAlignment = VerticalAlignment.Center;
                Canvas.SetLeft(ellipse, correctPosition(Simulation.GetX(i), 0, 0));
                Canvas.SetTop(ellipse, correctPosition(Simulation.GetY(i), 0, 0));
                ellipseCollection.Add(ellipse);
                Canvas.Children.Add(ellipse);
            }
        }
        public override void Start(int BallsNumber) 
        {
            Simulation.Start();
            CreateEllipses(BallsNumber);
        }
        public override void Stop() 
        {
            Simulation.Stop();
        }
        public override void MoveAll()
        {
            for (int i = 0; i < Simulation.GetBallCount(); i++)
            {
                Canvas.SetLeft(ellipseCollection[i], correctPosition(Simulation.GetX(i), 0, 0));
                Canvas.SetTop(ellipseCollection[i], correctPosition(Simulation.GetY(i), 0, 0));
            }

        }

        public override void Move(int index)
        {
            int left = correctPosition(Simulation.GetX(index),0, 0);
            int right = correctPosition(Simulation.GetY(index), 0, 0);
            Application.Current.Dispatcher.Invoke(new Action (() => {
                Canvas.SetLeft(ellipseCollection[index], left);
                Canvas.SetTop(ellipseCollection[index], right);
            }));
        }


        private int correctPosition(int Value, int StartOffset, int EndOffset)
        {
            int newValue = Value - BallRadius;

            return newValue;
        }
    }
}
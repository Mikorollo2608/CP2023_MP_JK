using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic
{
    public class Ball : IDisposable, INotifyPropertyChanged
    {
        //PropertyChanged is fired when ball moves
        public event PropertyChangedEventHandler PropertyChanged;
        private Random random = new Random();
        public int X { get; set; }
        public int Y { get; set; }

        private Timer BallTimer;

        public Ball(int x, int y, PropertyChangedEventHandler function)
        {
            X = x;
            Y = y;
            BallTimer = new Timer(Move, null, 0, 150);
            PropertyChanged += function;
        }

        private void Move(object? state)
        {
            X += random.Next(21) - 10;
            Y += random.Next(21) - 10;
            OnPropertyChanged();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public PropertyChangedEventHandler GetPublisher() { return PropertyChanged; }

        public void Dispose() { BallTimer.Dispose(); PropertyChanged = null; }
    }
}
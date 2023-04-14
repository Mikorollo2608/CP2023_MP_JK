using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Model;

namespace ViewModel
{
    public class ViewModelMainWindow : ViewModelBase
    {
        private ModelAbstractApi ModelApi;
        private int _BallsNumber;

        public ICommand AddCommand { get; set; }
        public ICommand StopCommand { get; set; }

        public ViewModelMainWindow()
        {
            ModelApi = ModelAbstractApi.CreateApi(50, 500, 500);
            // ??
            // IDisposable observer = ModelApi.Subscribe<>
            AddCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);

            ModelApi.CreateEllipses(2);
        }



        public int BallsNumber
        {
            get { return _BallsNumber; }
            set
            {
                if (value == _BallsNumber)
                    return;
                _BallsNumber = value;
                RaisePropertyChanged();
            }
        }

        // ???
        // public ObservableCollection<>


        public Canvas Canvas
        {
            get
            {
                return ModelApi.Canvas;
            }
        }


        private void Start()
        {
            ModelApi.Start();
        }
        private void Stop()
        {
            ModelApi.Start();
        }

    }
}

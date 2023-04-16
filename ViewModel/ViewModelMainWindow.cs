using System.Windows.Controls;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class ViewModelMainWindow : ViewModelBase
    {
        private ModelAbstractApi ModelApi;
        private int _BallsNumber = 0;

        public ICommand AddCommand { get; set; }
        public ICommand StopCommand { get; set; }

        public ViewModelMainWindow()
        {
            ModelApi = ModelAbstractApi.CreateApi(25, 500, 500);
            AddCommand = new RelayCommand(Start);
            StopCommand = new RelayCommand(Stop);
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

        public Border Border
        {
            get
            {
                return ModelApi.Border;
            }
        }

        private void Start()
        {
            ModelApi.Start(BallsNumber);
            BallsNumber = 0;
        }
        private void Stop()
        {
            ModelApi.Stop();
        }

    }
}

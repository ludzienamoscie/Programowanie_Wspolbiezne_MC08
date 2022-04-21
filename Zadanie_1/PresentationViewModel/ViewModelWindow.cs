using System.Collections;
using System.Windows.Input;
using PresentationModel;

namespace PresentationViewModel
{
    public class ViewModelWindow : ViewModelBase
    {
        private int _ballNumber;
        private readonly int _rectWidth;
        private readonly int _rectHeight;
        private readonly Model _model;
        private IList _balls;

        public ViewModelWindow() : this(Model.CreateApi()) { }

        public ViewModelWindow(Model model)
        {
            _model = model;
            _rectWidth = model.RectWidth;
            _rectHeight = model.RectHeight;
            Start = new RelayCommand(() => StartAction());
            Stop = new RelayCommand(() => StopAction());
        }
        public int BallNumber
        {
            get => _ballNumber;
            set
            {
                if (value.Equals(_ballNumber))
                    return;
                if (value < 0)
                    value = 0;
                if (value > 2137)
                    value = 2137;
                _ballNumber = value;
                RaisePropertyChanged(nameof(BallNumber));
            }
        }

        public ICommand Start { get; set; }
        public ICommand Stop { get; set; }

        public void StartAction()
        {
            _model.Stop();
            Balls = _model.Balls(_ballNumber);
            _model.Move(Balls);
        }
        public void StopAction()
        {
            _model.Stop();
        }
        public int RectWidth 
        { 
            get => _rectWidth;
        }
        public int RectHeight
        {
            get => _rectHeight;
        }
        public IList Balls
        {
            get => _balls;
            set
            {
                if (value.Equals(_balls))
                    return;
                _balls = value;
                RaisePropertyChanged(nameof(Balls));
            }
        }
    }
}

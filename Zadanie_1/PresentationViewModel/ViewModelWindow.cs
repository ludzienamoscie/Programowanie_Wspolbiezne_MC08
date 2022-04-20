using System.Windows.Input;
using PresentationModel;

namespace PresentationViewModel
{
    public class ViewModelWindow : ViewModelBase
    {
        private uint _ballNumber;
        private readonly int _rectWidth;
        private readonly int _rectHeight;
        private readonly Model _model;
        private object _balls;

        public ViewModelWindow() : this(Model.CreateApi()) { }

        public ViewModelWindow(Model model)
        {
            _model = model;
            _rectWidth = model.RectWidth;
            _rectHeight = model.RectHeight;
            Start = new RelayCommand(() => StartAction());
        }
        public uint BallNumber
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

        public void StartAction()
        {
            Balls = _model.Balls(_ballNumber);
        }
        public int RectWidth 
        { 
            get => _rectWidth;
        }

        public int RectHeight
        {
            get => _rectHeight;
        }
        public object Balls
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

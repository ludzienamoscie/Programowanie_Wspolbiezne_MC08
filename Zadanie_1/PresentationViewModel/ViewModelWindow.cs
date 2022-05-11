using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using PresentationModel;

namespace PresentationViewModel
{
    public class ViewModelWindow : ViewModelBase
    {
        private int _ballNumber;
        private readonly double _rectWidth;
        private readonly double _rectHeight;
        private readonly double _stroke;
        private readonly Model _model;
        private IList _balls;

        public ViewModelWindow() : this(Model.CreateApi()) { }

        public ViewModelWindow(Model model)
        {
            _model = model;
            _rectWidth = model.RectWidth;
            _rectHeight = model.RectHeight;
            _stroke = model.Stroke;
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
            Thread.Sleep(30);
            Balls = _model.Balls(_ballNumber);
            _model.Start(Balls);
        }
        public void StopAction()
        {
            _model.Stop();
        }
        public double RectWidth 
        { 
            get => _rectWidth;
        }
        public double RectHeight
        {
            get => _rectHeight;
        }
        public double Stroke
        {
            get => _stroke;
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

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic
{
    public class Ball : INotifyPropertyChanged
    {
        private Vector2D _v;
        private readonly double _r;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Vector2D V 
        { 
            get => _v;
            set
            {
                if (value.Equals(_v))
                    return;
                _v = value;
                RaisePropertyChanged(nameof(V));
            }
        }
        public double R => _r;

        public Ball(double x, double y, double r)
        {
            _v = new Vector2D(x, y);
            _r = r;
        }
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic
{
    public class Ball : INotifyPropertyChanged
    {
        private Vector2D _v;
        private readonly double _r;
        private readonly double _m;

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

        public double M => _m;

        public Ball(double x, double y, double r, double m)
        {
            _v = new Vector2D(x, y);
            _r = r;
            _m = m;
        }

        public Ball(Ball b)
        {
            if (b != null)
            {
                _r = b.R;
                _m = b.M;
                _v = new Vector2D(b.V);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Ball other)
            {
                if (other.V.Equals(V) && other.R == R && other.M == M)
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
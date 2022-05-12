using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Data
{
    public class Ball : INotifyPropertyChanged
    {
        private Vector2D _position;
        private Vector2D _velocity;
        private readonly double _r;
        private readonly double _m;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Vector2D Position 
        { 
            get => _position;
            set
            {
                if (value.Equals(_position))
                    return;
                _position = value;
                RaisePropertyChanged(nameof(Position));
            }
        }

        public Vector2D Velocity
        {
            get => _velocity;
            set
            {
                if (value.Equals(_velocity))
                    return;
                _velocity = value;
                RaisePropertyChanged(nameof(Velocity));
            }
        }
        public double R => _r;

        public double M => _m;

        public Ball(double x, double y, double r, double m)
        {
            _position = new Vector2D(x, y);
            _velocity = new Vector2D(0, 0);
            _r = r;
            _m = m;
        }

        public Ball(double x, double y, double r, double m, double vx, double vy)
        {
            _position = new Vector2D(x, y);
            _velocity = new Vector2D(vx, vy);
            _r = r;
            _m = m;
        }

        public Ball(Ball b)
        {
            if (b != null)
            {
                _r = b.R;
                _m = b.M;
                _position = new Vector2D(b.Position);
                _velocity = new Vector2D(b.Velocity);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Ball other)
            {
                if (other.Position.Equals(Position) && other.R == R && other.M == M)
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
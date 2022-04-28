using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic
{
    public class Vector2D : INotifyPropertyChanged
    {
        private double _x;
        private double _y;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double X 
        { 
            get => _x;
            set
            {
                if (value.Equals(_x))
                    return;
                _x = value;
                RaisePropertyChanged(nameof(X));
            }
        }
        public double Y 
        { 
            get => _y;
            set
            {
                if (value.Equals(_y))
                    return;
                _y = value;
                RaisePropertyChanged(nameof(Y));
            }
        }

        public Vector2D(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public Vector2D(Vector2D v)
        {
            if (v != null)
            {
                _x = v.X;
                _y = v.Y;
            }
        }

        public Vector2D Add(Vector2D other)
        {
            return new Vector2D(this.X + other.X, this.Y + other.Y);
        }

        public Vector2D MultiplyByScalar(double scalar)
        {
            return new Vector2D(scalar * this.X, scalar * this.Y);
        }

        // Iloczyn skalarny
        public static double DotProduct(Vector2D v1, Vector2D v2)
        {
            return v1.X * v1.X + v2.Y * v2.Y;
        }

        // Długość wektora
        public double MagnitudeSquared()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        public static Vector2D operator+(Vector2D v1, Vector2D v2)
        {
            return v1.Add(v2);
        }

        public static Vector2D operator-(Vector2D v1, Vector2D v2)
        {
            return v1.Add(v2.MultiplyByScalar(-1));
        }

        public static Vector2D operator*(double scalar, Vector2D v)
        {
            return v.MultiplyByScalar(scalar);
        }
        public static Vector2D operator*(Vector2D v, double scalar)
        {
            return scalar * v;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Vector2D other)
            {
                if (this.X == other.X && this.Y == other.Y)
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

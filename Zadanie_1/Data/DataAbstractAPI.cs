using System.Collections;
using System.Collections.ObjectModel;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public static DataAbstractAPI CreateBallData()
        {
            return new DataAPI();
        }
        public abstract double RectWidth { get; }
        public abstract double RectHeight { get; }
        public abstract double Stroke { get; }
        public abstract double MassLimit { get; }
        public abstract ObservableCollection<Ball> CreateBalls(int number, double XLimit, double YLimit, double Stroke, double MLimit);
    }
}

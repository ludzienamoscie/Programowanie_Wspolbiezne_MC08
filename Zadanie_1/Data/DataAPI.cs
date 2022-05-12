using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace Data
{
    internal class DataAPI : DataAbstractAPI
    {
        private ObservableCollection<Ball> _ballList;
        public override double RectWidth => 785;
        public override double RectHeight => 320;
        public override double Stroke => 10;
        public override double MassLimit => 100;
        public override ObservableCollection<Ball> CreateBalls(int number, double XLimit, double YLimit, double Stroke, double MLimit)
        {
            _ballList = new ObservableCollection<Ball>();

            double x, y, r, m, vx, vy;
            double speed = 2;
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                Thread.Sleep(1);
                r = 20;
                x = random.Next((int)Stroke, (int)(XLimit - r) - 1) + random.NextDouble();
                y = random.Next((int)Stroke, (int)(YLimit - r) - 1) + random.NextDouble();
                m = random.Next(1, (int)MLimit - 1) + random.NextDouble();
                vx = random.NextDouble() * speed * (random.Next(0, 1) == 0 ? -1 : 1);
                vy = Math.Sqrt(speed * speed - vx * vx) * (random.Next(0, 1) == 0 ? -1 : 1);
                _ballList.Add(new Ball(x, y, r, m, vx, vy));
            }
            return _ballList;
        }
    }
}

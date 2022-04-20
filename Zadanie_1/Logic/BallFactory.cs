using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Logic
{
    public class BallFactory
    {
        public BallFactory()
        {
        }

        public static ObservableCollection<Ball> CreateBalls(uint number, float XLimit, float YLimit)
        {
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
            float x, y, r;
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                r = (float)random.Next(10, 40) + (float)random.NextDouble();
                x = (float)random.Next(10, (int)(XLimit - r) - 1) + (float)random.NextDouble();
                y = (float)random.Next(10, (int)(YLimit - r) - 1) + (float)random.NextDouble();

                ballList.Add(new Ball(x, y, r));
            }
            return ballList;
        }
    }
}

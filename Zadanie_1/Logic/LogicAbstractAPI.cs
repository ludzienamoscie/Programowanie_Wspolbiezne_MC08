using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public abstract double RectWidth { get; }
        public abstract double RectHeight { get; }
        public abstract double Stroke { get; }
        public abstract double MassLimit { get; }
        public static LogicAbstractAPI CreateBallAPI() => new BallFactory();
        public abstract IList CreateBalls(int number, double XLimit, double YLimit, double Stroke, double MLimit);
        public abstract void Dance(IList balls, double XLimit, double YLimit, double Stroke);
        public abstract void EndOfTheParty(IList balls);
    }
}

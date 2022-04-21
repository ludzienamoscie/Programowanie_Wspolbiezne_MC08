﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI CreateBallAPI() => new BallFactory();
        public abstract IList CreateBalls(int number, double XLimit, double YLimit);
        public abstract void Dance(IList balls, double XLimit, double YLimit);
        public abstract void EndOfTheParty();
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using System;
using System.Collections.ObjectModel;
using System.Threading;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {
        [TestMethod]
        public void StepsTest()
        {
            BallFactory ballFactory = new BallFactory();
            ObservableCollection<Ball> balls = (ObservableCollection<Ball>)ballFactory.CreateBalls(1, 100, 100, 10, 100);
            Ball ball = new Ball(balls[0]);
            ballFactory.Dance(balls, 500, 500, 10);
            // Czekanie na zmiane wspolrzednych
            Thread.Sleep(50);
            Assert.AreNotEqual(balls[0], ball);
            ballFactory.EndOfTheParty();
            Thread.Sleep(100);
            ball = new Ball(balls[0]);
            // Czekanie na zmiane wspolrzednych
            Thread.Sleep(50);
            Assert.AreEqual(balls[0], ball);
        }
    }
}

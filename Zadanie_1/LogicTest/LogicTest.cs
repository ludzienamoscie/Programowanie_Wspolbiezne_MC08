using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using Data;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

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
            ballFactory.EndOfTheParty(balls);
            Thread.Sleep(100);
            ball = new Ball(balls[0]);
            // Czekanie na zmiane wspolrzednych
            Thread.Sleep(50);
            Assert.AreEqual(balls[0], ball);
        }
        [TestMethod]
        public void CollisionBallsTest()
        {
            BallFactory ballFactory = new BallFactory();
            IList balls = new List<Ball>();
            Ball ball1 = new Ball(100, 100, 20, 10, 0.5, 2);
            Ball ball2 = new Ball(110, 110, 20, 20, -1, -1.5);
            balls.Add(ball1);
            balls.Add(ball2);
            ballFactory.LookForCollisionsNaive(balls, ball1);
            Assert.IsTrue(ball1.Velocity.X != 0.5 && ball1.Velocity.Y != 2);
        }
    }
}

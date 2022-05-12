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
        public void EuclideanTest()
        {
            BallFactory ballFactory = new BallFactory();
            Vector2D starting_p = new Vector2D(0, 0);
            Vector2D end_p = new Vector2D(1, 1);
            double actual_dist = ballFactory.EuklideanDist(starting_p, end_p);
            Assert.AreEqual(2, actual_dist * actual_dist, 0.0001);
        }

        [TestMethod]
        public void LinearFactorsTest()
        {
            BallFactory ballFactory = new BallFactory();
            Vector2D point_1 = new Vector2D(0, 1);
            Vector2D point_2 = new Vector2D(1, 1.5);
            double a, b;
            (a, b) = ballFactory.LinearFactors(point_1, point_2);
            Assert.AreEqual(0.5, a, 0.0001);
            Assert.AreEqual(1, b, 0.0001);
        }

        [TestMethod]
        public void QuadRootsTest()
        {
            BallFactory ballFactory = new BallFactory();
            double sol1, sol2;
            (sol1, sol2) = ballFactory.QuadRoots(1, 3, 2);
            Assert.AreEqual(sol1, -1, 0.0001);
            Assert.AreEqual(sol2, -2, 0.0001);
        }

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

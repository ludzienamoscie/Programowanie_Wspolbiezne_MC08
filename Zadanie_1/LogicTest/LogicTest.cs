using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System;
using System.Collections.ObjectModel;

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
        public void CreateBallsTest()
        {
            BallFactory ballFactory = new BallFactory();
            int xlim = 100;
            int ylim = 100;
            ObservableCollection<Ball> ballList = ballFactory.CreateBalls(100, xlim, ylim);
            foreach (Ball ball in ballList)
            {
                Assert.IsTrue(ball.V.X < xlim);
                Assert.IsTrue(ball.V.Y < ylim);
                foreach (Ball ball2 in ballList)
                {
                    if (ball2 == ball)
                    {
                        continue;
                    }
                    Assert.IsFalse(ball2.V.X == ball.V.X && ball2.V.Y == ball.V.Y);
                }
            }
        }
        [TestMethod]

        public void MoveTest()
        {
            BallFactory ballFactory = new BallFactory();
            ObservableCollection<Ball> balls = ballFactory.CreateBalls(1, 100, 100);
            Ball ball = balls[0];
            ball.V.X = 1;
            ball.V.Y = 1;
            Vector2D targetPos = new Vector2D(2, 2);
            ballFactory.Move(ball, targetPos, Math.Sqrt(2));
            Assert.AreEqual(2, ball.V.X, 0.0001);
            Assert.AreEqual(2, ball.V.Y, 0.0001);

            // Nad kulą
            targetPos = new Vector2D(2, 3);
            ballFactory.Move(ball, targetPos, 1);
            Assert.AreEqual(2, ball.V.X, 0.0001);
            Assert.AreEqual(3, ball.V.Y, 0.0001);
        }
    }
}

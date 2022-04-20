using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System;

namespace LogicTest
{
    [TestClass]
    public class UnitTest1
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
    }
}

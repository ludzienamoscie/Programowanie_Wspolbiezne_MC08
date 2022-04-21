using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;
using System;
using System.Collections.ObjectModel;
using System.Collections;

namespace LogicTest
{
    [TestClass]
    public class LogicTest
    {

        [TestMethod]
        public void CreateBallsTest()
        {
            LogicAbstractAPI ballFactory = LogicAbstractAPI.CreateBallAPI();
            int xlim = 100;
            int ylim = 100;
            IList ballList = ballFactory.CreateBalls(100, xlim, ylim);
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
    }
}

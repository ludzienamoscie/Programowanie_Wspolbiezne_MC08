using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.Collections.ObjectModel;

namespace DataTest
{
    [TestClass]
    public class DataTest
    {
        [TestMethod]
        public void CreateBallsTest()
        {
            DataAPI _dataAPI = new DataAPI();
            int xlim = 100;
            int ylim = 100;
            ObservableCollection<Ball> ballList = (ObservableCollection<Ball>) _dataAPI.CreateBalls(100, xlim, ylim, 10, 100);
            foreach (Ball ball in ballList)
            {
                Assert.IsTrue(ball.Position.X < xlim);
                Assert.IsTrue(ball.Position.Y < ylim);
            }
        }
    }
}

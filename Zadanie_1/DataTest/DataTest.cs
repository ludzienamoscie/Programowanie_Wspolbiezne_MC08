using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using System.Collections.ObjectModel;
using System.IO;

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
                // Stroke == 10
                Assert.IsTrue(ball.Position.X >= 10);
                Assert.IsTrue(ball.Position.Y >= 10);
            }
        }

        [TestMethod]
        public void FileExistTest()
        {
            DataAPI _dataAPI = new DataAPI();
            _dataAPI.AppendObjectToJSONFile(@"test.json", "");
            Assert.IsTrue(File.Exists(@"test.json"));
        }
    }
}

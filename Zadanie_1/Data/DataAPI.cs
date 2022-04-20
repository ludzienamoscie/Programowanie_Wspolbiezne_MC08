namespace Data
{
    public class DataAPI
    {
        public static DataAPI CreateBallData()
        {
            return new BallData();
        }
        private class BallData : DataAPI
        {
            //Nothing
        }
    }
}

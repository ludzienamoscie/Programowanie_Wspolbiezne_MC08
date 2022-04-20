using System.Collections;
using System.Collections.ObjectModel;
using Logic;

namespace PresentationModel
{
    public abstract class Model
    {
        public abstract int RectWidth { get; }
        public abstract int RectHeight { get; }
        public abstract ObservableCollection<Ball> Balls(int ballNumber);
        public abstract void Move(IList balls);
        public abstract void Stop();
        public static Model CreateApi()
        {
            return new ModelAPI();
        }

    }
    internal class ModelAPI : Model
    {
        private readonly BallFactory factory = new BallFactory();
        public override int RectWidth => 760;
        public override int RectHeight => 310;
        public override ObservableCollection<Ball> Balls(int ballNumber) 
            => factory.CreateBalls(ballNumber, RectWidth, RectHeight);
        public override void Move(IList balls)
        {
            factory.Dance((ObservableCollection<Ball>)balls, RectWidth, RectHeight);
        }
        public override void Stop() => factory.EndOfTheParty();
    }
}

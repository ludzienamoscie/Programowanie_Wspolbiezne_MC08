using System;
using System.Collections.ObjectModel;
using Logic;

namespace PresentationModel
{
    public abstract class Model
    {
        public abstract int RectWidth { get; }
        public abstract int RectHeight { get; }
        public abstract ObservableCollection<Ball> Balls(uint ballNumber);

        public static Model CreateApi()
        {
            return new ModelAPI();
        }

    }
    internal class ModelAPI : Model
    {
        public override int RectWidth => 760;
        public override int RectHeight => 310;
        public override ObservableCollection<Ball> Balls(uint ballNumber) 
            => BallFactory.CreateBalls(ballNumber, RectWidth, RectHeight);
    }
}

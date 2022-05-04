using System.Collections;
using System.Collections.ObjectModel;
using Logic;

namespace PresentationModel
{
    public abstract class Model
    {
        public abstract double RectWidth { get; }
        public abstract double RectHeight { get; }
        public abstract double Stroke { get; }
        public abstract double MassLimit { get; }
        public abstract IList Balls(int ballNumber);
        public abstract void Start(IList balls);
        public abstract void Stop();
        public static Model CreateApi()
        {
            return new ModelAPI();
        }
    }
}

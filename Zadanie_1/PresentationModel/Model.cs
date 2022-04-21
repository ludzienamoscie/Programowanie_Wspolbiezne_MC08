using System.Collections;
using System.Collections.ObjectModel;
using Logic;

namespace PresentationModel
{
    public abstract class Model
    {
        public abstract int RectWidth { get; }
        public abstract int RectHeight { get; }
        public abstract IList Balls(int ballNumber);
        public abstract void Start(IList balls);
        public abstract void Stop();
        public static Model CreateApi()
        {
            return new ModelAPI();
        }
    }
}

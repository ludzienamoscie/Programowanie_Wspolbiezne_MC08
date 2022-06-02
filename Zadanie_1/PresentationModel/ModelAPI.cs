using Logic;
using System.Collections;
using System.Collections.ObjectModel;

namespace PresentationModel
{
    internal class ModelAPI : Model
    {
        private readonly LogicAbstractAPI _logic;
        public override double RectWidth => _logic.RectWidth;
        public override double RectHeight => _logic.RectHeight;
        public override double Stroke => _logic.Stroke;
        public override double MassLimit => _logic.MassLimit;
        public override IList Balls(int ballNumber) 
            => _logic.CreateBalls(ballNumber, RectWidth, RectHeight, Stroke, MassLimit);
        public override void Start(IList balls)
        {
            _logic.Dance(balls, RectWidth, RectHeight, Stroke);
        }
        public override void Stop(IList balls) => _logic.EndOfTheParty(balls);

        public ModelAPI() : this(LogicAbstractAPI.CreateBallAPI()) { }
        public ModelAPI(LogicAbstractAPI logic) { _logic = logic; }
    }
}

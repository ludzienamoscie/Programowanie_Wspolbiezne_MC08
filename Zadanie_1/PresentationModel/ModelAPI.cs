using Logic;
using System.Collections;
using System.Collections.ObjectModel;

namespace PresentationModel
{
    internal class ModelAPI : Model
    {
        private readonly LogicAbstractAPI _logic;
        public override double RectWidth => 785;
        public override double RectHeight => 320;
        public override double Stroke => 10;
        public override double MassLimit => 100;
        public override IList Balls(int ballNumber) 
            => _logic.CreateBalls(ballNumber, RectWidth, RectHeight, Stroke, MassLimit);
        public override void Start(IList balls)
        {
            _logic.Dance(balls, RectWidth, RectHeight, Stroke);
        }
        public override void Stop() => _logic.EndOfTheParty();

        public ModelAPI() : this(LogicAbstractAPI.CreateBallAPI()) { }
        public ModelAPI(LogicAbstractAPI logic) { _logic = logic; }
    }
}

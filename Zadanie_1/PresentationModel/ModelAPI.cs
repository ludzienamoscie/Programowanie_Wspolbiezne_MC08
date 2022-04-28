using Logic;
using System.Collections;
using System.Collections.ObjectModel;

namespace PresentationModel
{
    internal class ModelAPI : Model
    {
        private readonly LogicAbstractAPI _logic;
        public override int RectWidth => 760;
        public override int RectHeight => 310;
        public override int MassLimit => 100;
        public override IList Balls(int ballNumber) 
            => _logic.CreateBalls(ballNumber, RectWidth, RectHeight, MassLimit);
        public override void Start(IList balls)
        {
            _logic.Dance(balls, RectWidth, RectHeight);
        }
        public override void Stop() => _logic.EndOfTheParty();

        public ModelAPI() : this(LogicAbstractAPI.CreateBallAPI()) { }
        public ModelAPI(LogicAbstractAPI logic) { _logic = logic; }
    }
}

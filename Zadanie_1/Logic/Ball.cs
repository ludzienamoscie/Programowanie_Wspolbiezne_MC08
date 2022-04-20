namespace Logic
{
    public class Ball
    {
        private float _x;
        private float _y;
        private readonly float _r;

        public float X { get => _x; set => _x = value; }
        public float Y { get => _y; set => _y = value; }
        public float R => _r;

        public Ball(float x, float y, float r)
        {
            _x = x;
            _y = y;
            _r = r;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Data;


[assembly: InternalsVisibleTo("LogicTest")]

namespace Logic
{
    internal class BallFactory : LogicAbstractAPI
    {
        private readonly DataAbstractAPI _data;
        private List<Task> _tasks;
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        // Kula ballList[i] może się prouszać tylko wtedy gdy movingMask[i] == true
        private bool[] _movingMask;
        public BallFactory() : this(DataAbstractAPI.CreateBallData()) { }
        public BallFactory(DataAbstractAPI data) { _data = data; }
        // Tworzenie kul
        public override IList CreateBalls(int number, double XLimit, double YLimit, double MLimit)
        {
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
            _movingMask = new bool[number];

            _tasks = new List<Task>();
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            double x, y, r, m, vx, vy;
            int maxSpeed = 3;
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                _movingMask[i] = true;
                Thread.Sleep(1);
                r = 20;
                x = random.Next(10, (int)(XLimit - r) - 1) + random.NextDouble();
                y = random.Next(10, (int)(YLimit - r) - 1) + random.NextDouble();
                m = random.Next(1, (int)MLimit - 1) + random.NextDouble();
                vx = random.NextDouble() * maxSpeed * (random.Next(0, 1) == 0 ? -1 : 1);
                vy = (maxSpeed - vx*vx) * (random.Next(0, 1) == 0 ? -1 : 1);
                ballList.Add(new Ball(x, y, r, m, vx, vy));
            }
            return ballList;
        }
        // Zatrzymanie kul
        public override void EndOfTheParty()
        {
            if (_tokenSource != null && !_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Cancel();
            }
        }

        // Rozpoczecie ruchu kul
        public override void Dance(IList balls, double XLimit, double YLimit)
        {
            int i = 0;
            foreach (Ball ball in balls)
            {
                _tasks.Add(Task.Run(() => Rolling(balls, XLimit, YLimit, i++)));
            }
            _tasks.Add(Task.Run(() => LookForCollisionsNaive(balls)));
        }
        public async void Rolling(IList balls, double XLimit, double YLimit, int ballIndex)
        {
            Ball ball = (Ball)balls[ballIndex];
            double r = ball.R / 2;
            while (true)
            {
                await Task.Delay(20);
                // Jeśli nie może się poruszać
                if (ballIndex >= _movingMask.Length || !_movingMask[ballIndex])
                {
                    continue;
                }
                // Odbicie od prawej ściany
                if (ball.Position.X + ball.Velocity.X + r > XLimit - 5)
                {
                    ball.Position.X = XLimit - r - 5;
                    ball.Velocity.X *= -1;
                }
                // Odbicie od lewej ściany
                else if (ball.Position.X + ball.Velocity.X - r < 5)
                {
                    ball.Position.X = 5 + r;
                    ball.Velocity.X *= -1;
                }
                else
                {
                    ball.Position.X += ball.Velocity.X;
                }

                // Odbicie od dolnej ściany
                if (ball.Position.Y + ball.Velocity.Y + r > YLimit - 5)
                {
                    ball.Position.Y = YLimit - r - 5;
                    ball.Velocity.Y *= -1;
                }
                // Odbicie od górnej ściany
                else if (ball.Position.Y + ball.Velocity.Y - r < 5)
                {
                    ball.Position.Y = 5 + r;
                    ball.Velocity.Y *= -1;
                }
                else
                {
                    ball.Position.Y += ball.Velocity.Y;
                }
                // Sprawdzenie, czy nalezy zatrzymac kule
                try { _token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
            }
        }

        // Iterowanie, bez drzewa
        public async void LookForCollisionsNaive(IList balls)
        {
            while (true)
            { 
                // Iteruje po wszystkich parach kul. Jeśli są wystarczająco blisko siebie, to zatrzymuje ich ruch, oblicza 
                // prędkość po zderzeniu i wznawia ruch
                for (int i = 0; i < _movingMask.Length; i++)
                {
                    for (int j = i + 1; j < _movingMask.Length; j++)
                    {
                        Ball ball1 = (Ball)balls[i];
                        Ball ball2 = (Ball)balls[j];
                        Vector2D relativePosition = ball1.Position - ball2.Position;
                        double distance = Math.Sqrt(relativePosition.MagnitudeSquared());
                        if (distance * 2< ball1.R + ball2.R)
                        {
                            _movingMask[i] = false;
                            _movingMask[j] = false;
                            Vector2D newV1, newV2;
                            (newV1, newV2) = EllasticCollision(ball1.M, ball2.M, ball1.Velocity, ball2.Velocity, ball1.Position, ball2.Position);
                            ball1.Velocity = newV1;
                            ball2.Velocity = newV2;
                            _movingMask[i] = true;
                            _movingMask[j] = true;
                        }                        
                    }
                }
                // Sprawdzenie, czy nalezy zatrzymac kule
                try { _token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { return; }
            }
        }
        
        // Pierwiastki funkcji kwadratowej
        public (double x1, double x2) QuadRoots(double a, double b, double c)
        {
            double delta = b * b - 4 * a * c;
            double x1, x2;
            x1 = (-b + Math.Sqrt(delta)) / (2 * a);
            x2 = (-b - Math.Sqrt(delta)) / (2 * a);
            return (x1, x2);
        }
        // Obliczanie parametrow funkcji liniowej przechodzacej przez 2 punkty
        public (double a, double b) LinearFactors(Vector2D A, Vector2D B)
        {
            double a = (B.Y - A.Y) / (B.X - A.X);
            double b = A.Y - a * A.X;
            return (a, b);
        }
        // Odleglosc euklidesowa
        public double EuklideanDist(Vector2D A, Vector2D B)
        {
            return Math.Sqrt( (A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y) );
        }

        public (Vector2D, Vector2D) EllasticCollision(double m1, double m2, Vector2D v1, Vector2D v2, Vector2D pos1, Vector2D pos2)
        {
            Vector2D relativeVelocity = v2 - v1;
            Vector2D relativePos = pos2 - pos1;
            // Jeśli nie lecą na siebie
            if (Vector2D.DotProduct(relativePos, relativeVelocity) > 0)
            {
                return (v1, v2);
            }
            Vector2D newV1 = v1 - 2 * m2 / (m1 + m2) * Vector2D.DotProduct(v1 - v2, pos1 - pos2) / (pos1 - pos2).MagnitudeSquared() * (pos1 - pos2);
            Vector2D newV2 = v2 - 2 * m1 / (m1 + m2) * Vector2D.DotProduct(v2 - v1, pos2 - pos1) / (pos2 - pos1).MagnitudeSquared() * (pos2 - pos1);
            if (Double.IsNaN(v1.X) || Double.IsNaN(v2.X) || Double.IsNaN(v1.Y) || Double.IsNaN(v2.Y))
            {
                return (v1, v2);
            }
            return (newV1, newV2);
        }
    }
}

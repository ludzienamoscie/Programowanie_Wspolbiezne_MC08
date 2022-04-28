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
        public BallFactory() : this(DataAbstractAPI.CreateBallData()) { }
        public BallFactory(DataAbstractAPI data) { _data = data; }
        // Tworzenie kul
        public override IList CreateBalls(int number, double XLimit, double YLimit, double MLimit)
        {
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
            _tasks = new List<Task>();
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            double x, y, r, m, vx, vy;
            int maxSpeed = 3;
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
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
            foreach (Ball ball in balls)
            {
                _tasks.Add(Task.Run(() => Rolling(ball, XLimit, YLimit)));
            }
        }
        public async void Rolling(Ball ball, double XLimit, double YLimit)
        {
            double r = ball.R / 2;
            while (true)
            {
                await Task.Delay(20);
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

        // Rozpoczecie ruchu kul
        public void Dance1(IList balls, double XLimit, double YLimit)
        {
            Random random = new Random();
            double x, y;
            foreach (Ball ball in balls)
            {
                x = random.Next(10, (int)(XLimit - ball.R) - 1) + random.NextDouble();
                y = random.Next(10, (int)(YLimit - ball.R) - 1) + random.NextDouble();
                Thread.Sleep(1);
                _tasks.Add(Task.Run(() => Steps(ball, new Vector2D(x, y), XLimit, YLimit)));
            }
        }
        // Plynne przemieszczanie sie kuli do punktu v
        public async void Steps(Ball ball, Vector2D destination, double XLimit, double YLimit)
        {
            Random random = new Random((int)(destination.X * destination.Y));
            double x, y;
            // Jak duzy krok
            double dash = 3;
            // Jak szybko robi krok
            int speed = 15;
            while (true)
            {
                await Task.Delay(speed);
                if (EuklideanDist(destination, ball.Position) > dash)
                {
                    Move(ball, destination, dash);
                }
                else
                {
                    ball.Position = destination;
                    x = random.Next(10, (int)(XLimit - ball.R) - 1) + random.NextDouble();
                    y = random.Next(10, (int)(YLimit - ball.R) - 1) + random.NextDouble();
                    destination = new Vector2D(x, y);
                }
                // Sprawdzenie, czy nalezy zatrzymac kule
                try { _token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
            }
        }
        // Wyznaczanie kolejnych punktow odleglych od A o r w strone B
        public void Move(Ball A, Vector2D B, double r)
        {
            double currDist = EuklideanDist(A.Position, B);
            double a, b, c, d, e, x1, x2;
            if (A.Position.X != B.X)
            {
                (a, b) = LinearFactors(A.Position, B);
            }
            else // Przypadek, gdy punkt B jest nad lub pod punktem A
            {
                if (currDist > EuklideanDist(B, new Vector2D(A.Position.X, A.Position.Y + 1)))
                    A.Position.Y++;
                else
                    A.Position.Y--;
                return;
            }
            c = a * a + 1;
            d = 2 * a * (b - A.Position.Y) - 2 * A.Position.X;
            e = A.Position.X * A.Position.X - r * r + (b - A.Position.Y) * (b - A.Position.Y);
            (x1, x2) = QuadRoots(c, d, e);
            if ( currDist > EuklideanDist(B, new Vector2D(x1, a * A.Position.X + b)) )
            {
                A.Position.X = x1;
                A.Position.Y = a * x1 + b;
            }
            else
            {
                A.Position.X = x2;
                A.Position.Y = a * x2 + b;
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

        // Zderzenie kul
        public (Vector2D, Vector2D) CollideBalls(Ball ball1, Ball ball2, Vector2D velocity1, Vector2D velocity2)
        {
            // Czy zderzają się
            Vector2D relative_position = ball2.Position - ball1.Position;
            // Kiedy są za daleko od siebie, nie zmieniaj prędkości
            if (relative_position.MagnitudeSquared() > ball1.R + ball2.R)
            {
                return (velocity1, velocity2);
            }
            
            Vector2D relative_velocity = velocity2 - velocity1;

            // Kiedy prędkość względna jest skierowana "na zewnątrz" (nie lecą na siebie), nie zmieniaj prędkości
            // Czyli kiedy kąt pomiędzy wektorem położenia względnego a wektorem względnej prędkości jest w zakresie [-pi/2, pi/2]
            // Wtedy i tylko wtedy iloczyn skalarny jest > 0
            if (Vector2D.DotProduct(relative_position, relative_velocity) > 0)
            {
                return (velocity1, velocity2);
            }
            Vector2D newVelocity1 = velocity1 - 2 * ball2.M / (ball1.M + ball2.M) * Vector2D.DotProduct(velocity1 - velocity2, ball1.Position - ball2.Position) / (ball1.Position - ball2.Position).MagnitudeSquared() * (ball1.Position - ball2.Position);
            Vector2D newVelocity2 = velocity2 - 2 * ball1.M / (ball1.M + ball2.M) * Vector2D.DotProduct(velocity2 - velocity1, ball2.Position - ball1.Position) / (ball2.Position - ball1.Position).MagnitudeSquared() * (ball2.Position - ball1.Position);
            return (newVelocity1, newVelocity2);
        }
    }
}

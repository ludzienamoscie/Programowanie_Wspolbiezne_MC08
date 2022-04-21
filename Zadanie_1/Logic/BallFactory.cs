using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    public class BallFactory
    {
        public List<Task> tasks;
        public CancellationToken token;
        public CancellationTokenSource tokenSource;
        public BallFactory()
        {
        }
        // Tworzenie kul
        public ObservableCollection<Ball> CreateBalls(int number, double XLimit, double YLimit)
        {
            ObservableCollection<Ball> ballList = new ObservableCollection<Ball>();
            tasks = new List<Task>();
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            double x, y, r;
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                r = random.Next(10, 40 - 1) + random.NextDouble();
                x = random.Next(10, (int)(XLimit - r) - 1) + random.NextDouble();
                y = random.Next(10, (int)(YLimit - r) - 1) + random.NextDouble();

                ballList.Add(new Ball(x, y, r));
            }
            return ballList;
        }
        // Zatrzymanie kul
        public void EndOfTheParty()
        {
            if (tokenSource != null && !tokenSource.IsCancellationRequested)
            {
                tokenSource.Cancel();
            }
        }
        // Rozpoczecie ruchu kul
        public void Dance(ObservableCollection<Ball> balls, double XLimit, double YLimit)
        {
            Random random = new Random();
            double x, y;
            foreach (Ball ball in balls)
            {
                x = random.Next(10, (int)(XLimit - ball.R) - 1) + random.NextDouble();
                y = random.Next(10, (int)(YLimit - ball.R) - 1) + random.NextDouble();
                Thread.Sleep(1);
                tasks.Add(Task.Run(() => Steps(ball, new Vector2D(x, y), XLimit, YLimit)));
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
                if (EuklideanDist(destination, ball.V) > dash)
                {
                    Move(ball, destination, dash);
                }
                else
                {
                    ball.V = destination;
                    x = random.Next(10, (int)(XLimit - ball.R) - 1) + random.NextDouble();
                    y = random.Next(10, (int)(YLimit - ball.R) - 1) + random.NextDouble();
                    destination = new Vector2D(x, y);
                }
                // Sprawdzenie, czy nalezy zatrzymac kule
                try { token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
            }
        }
        // Wyznaczanie kolejnych punktow odleglych od A o r w strone B
        public void Move(Ball A, Vector2D B, double r)
        {
            double currDist = EuklideanDist(A.V, B);
            double a, b, c, d, e, x1, x2;
            if (A.V.X != B.X)
            {
                (a, b) = LinearFactors(A.V, B);
            }
            else // Przypadek, gdy punkt B jest nad lub pod punktem A
            {
                if (currDist > EuklideanDist(B, new Vector2D(A.V.X, A.V.Y + 1)))
                    A.V.Y++;
                else
                    A.V.Y--;
                return;
            }
            c = a * a + 1;
            d = 2 * a * (b - A.V.Y) - 2 * A.V.X;
            e = A.V.X * A.V.X - r * r + (b - A.V.Y) * (b - A.V.Y);
            (x1, x2) = QuadRoots(c, d, e);
            if ( currDist > EuklideanDist(B, new Vector2D(x1, a * A.V.X + b)) )
            {
                A.V.X = x1;
                A.V.Y = a * x1 + b;
            }
            else
            {
                A.V.X = x2;
                A.V.Y = a * x2 + b;
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
    }
}

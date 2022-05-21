﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Data;


[assembly: InternalsVisibleTo("LogicTest")]

namespace Logic
{
    internal class BallFactory : LogicAbstractAPI
    {
        public override double RectWidth => _data.RectWidth;
        public override double RectHeight => _data.RectHeight;
        public override double Stroke => _data.Stroke;
        public override double MassLimit => _data.MassLimit;

        private readonly DataAbstractAPI _data;
        private List<Task> _tasks;
        private readonly object locker = new object();
        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;
        private string log_path = "ball_log.json";

        private class CollisionInfo
        {
            Vector2D _position_ball_1;
            Vector2D _position_ball_2;
            Vector2D _initial_vel_1;
            Vector2D _initial_vel_2;
            Vector2D _final_vel_1;
            Vector2D _final_vel_2;
            Ball _ball_1;
            Ball _ball_2;

            public CollisionInfo(Vector2D position_ball_1,
            Vector2D position_ball_2,
            Vector2D initial_vel_1,
            Vector2D initial_vel_2,
            Vector2D final_vel_1,
            Vector2D final_vel_2,
            Ball ball_1,
            Ball ball_2)
            {
                this._position_ball_1 = position_ball_1;
                this._position_ball_2 = position_ball_2;
                this._initial_vel_1 = initial_vel_1;
                this._initial_vel_2 = initial_vel_2;
                this._final_vel_1 = final_vel_1;
                this._final_vel_2 = final_vel_2;
                this._ball_1 = ball_1;
                this._ball_2 = ball_2;
            }

            public Vector2D PositionBall1
            {
                get => _position_ball_1;
            }
            public Vector2D PositionBall2
            {
                get => _position_ball_2;
            }

            public Vector2D InitialVel1
            {
                get => _initial_vel_1;
            }

            public Vector2D InitialVel2
            {
                get => _initial_vel_2;
            }

            public Vector2D FinalVel1
            {
                get => _final_vel_1;
            }

            public Vector2D FinalVel2
            {
                get => _final_vel_2;
            }

            public Ball Ball_1
            {
                get => _ball_1;
            }

            public Ball Ball_2
            {
                get => _ball_2;
            }
        }
        public BallFactory() : this(DataAbstractAPI.CreateBallData()) { }
        public BallFactory(DataAbstractAPI data) { _data = data; }
        // Tworzenie kul
        public override IList CreateBalls(int number, double XLimit, double YLimit, double Stroke, double MLimit)
        {
            _tasks = new List<Task>();
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            return _data.CreateBalls(number, XLimit, YLimit, Stroke, MLimit);
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
        public override void Dance(IList balls, double XLimit, double YLimit, double Stroke)
        {
            foreach (Ball ball in balls)
            {
                _tasks.Add(Task.Run(() => Rolling(balls, XLimit, YLimit, Stroke, ball)));
            }
            _tasks.Add(Task.Run(() => callLogger(100, balls)));
        }
        public async void Rolling(IList balls, double XLimit, double YLimit, double Stroke, Ball ball)
        {
            while (true)
            {
                await Task.Delay(15);
                // Odbicie od prawej ściany
                if (ball.Position.X + ball.Velocity.X > XLimit - ball.R - Stroke)
                {
                    ball.Position.X = XLimit - ball.R - Stroke;
                    ball.Velocity.X *= -1;
                }
                // Odbicie od lewej ściany
                else if (ball.Position.X + ball.Velocity.X < Stroke)
                {
                    ball.Position.X = Stroke;
                    ball.Velocity.X *= -1;
                }
                else
                {
                    ball.Position.X += ball.Velocity.X;
                }

                // Odbicie od dolnej ściany
                if (ball.Position.Y + ball.Velocity.Y > YLimit - ball.R - Stroke)
                {
                    ball.Position.Y = YLimit - ball.R - Stroke;
                    ball.Velocity.Y *= -1;
                }
                // Odbicie od górnej ściany
                else if (ball.Position.Y + ball.Velocity.Y < Stroke)
                {
                    ball.Position.Y = Stroke;
                    ball.Velocity.Y *= -1;
                }
                else
                {
                    ball.Position.Y += ball.Velocity.Y;
                }
                // Sprawdzenie, czy nalezy zatrzymac kule
                try { _token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
                LookForCollisionsNaive(balls, ball);
            }
        }

        // Iterowanie, bez drzewa
        public void LookForCollisionsNaive(IList balls, Ball ball)
        {
            // Iteruje po wszystkich parach kul. Jeśli są wystarczająco blisko siebie, to zatrzymuje ich ruch, oblicza 
            // prędkość po zderzeniu i wznawia ruch
            foreach (Ball ball1 in balls)
            {
                if (ball1 == ball)
                    continue;
                Vector2D relativePosition = ball.Position - ball1.Position;
                double distance = Math.Sqrt(relativePosition.MagnitudeSquared());
                if (distance * 2 <= ball.R + ball1.R)
                {
                    EllasticCollision(ball, ball1);
                }
            }
        }

        public void EllasticCollision(Ball ball1, Ball ball2)
        {
            Vector2D relativeVelocity = ball2.Velocity - ball1.Velocity;
            Vector2D relativePos = ball2.Position - ball1.Position;
            // Jeśli nie lecą na siebie
            if (Vector2D.DotProduct(relativePos, relativeVelocity) > 0)
            {
                return;
            }
            Vector2D newV1 = ball1.Velocity - 2 * ball2.M / (ball1.M + ball2.M) * Vector2D.DotProduct(ball1.Velocity - ball2.Velocity, ball1.Position - ball2.Position) / (ball1.Position - ball2.Position).MagnitudeSquared() * (ball1.Position - ball2.Position);
            Vector2D newV2 = ball2.Velocity - 2 * ball1.M / (ball1.M + ball2.M) * Vector2D.DotProduct(ball2.Velocity - ball1.Velocity, ball2.Position - ball1.Position) / (ball2.Position - ball1.Position).MagnitudeSquared() * (ball2.Position - ball1.Position);
            if (double.IsNaN(ball1.Velocity.X) || double.IsNaN(ball2.Velocity.X) || double.IsNaN(ball1.Velocity.Y) || double.IsNaN(ball2.Velocity.Y))
            {
                return;
            }
            // Sekcja krytyczna
            lock (locker)
            {
                Vector2D initVel1 = ball1.Velocity;
                Vector2D initVel2 = ball2.Velocity;
                ball1.Velocity = newV1;
                ball2.Velocity = newV2;
                CollisionInfo collisionInfo = new CollisionInfo(ball1.Position, ball2.Position, initVel1, initVel2, ball1.Velocity, ball2.Velocity, ball1, ball2);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonCollisionInfo = JsonSerializer.Serialize(collisionInfo, options);
                string now = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");
                string newJsonObject = "{" + String.Format("\n\t\"datetime\": \"{0}\",\n\t\"collision\":{1}\n", now, jsonCollisionInfo) + "}";

                AppendObjectToJSONFile(log_path, newJsonObject);
            }
        }

        public void callLogger(int interval, IList balls)
        {
            while (true)
            {
                Thread.Sleep(interval);
                // Zatrzymaj log jeśli zatrzymano kule
                try { _token.ThrowIfCancellationRequested(); }
                catch (OperationCanceledException) { break; }
                logBalls(balls);
            }
        }

        public void logBalls(IList balls)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonBalls = JsonSerializer.Serialize(balls, options);
            string now = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");

            string newJsonObject = "{" + String.Format("\n\t\"datetime\": \"{0}\",\n\t\"balls\":{1}\n", now, jsonBalls) + "}";
            lock (locker)
            {
                AppendObjectToJSONFile(log_path, newJsonObject);
            }
        }

        private void AppendObjectToJSONFile(string filename, string newJsonObject)
        {
            if (!File.Exists(filename))
            {
                using (StreamWriter sw = new StreamWriter(filename, true))
                {
                    sw.WriteLine("[]");
                }
            }

            string content;
            using (StreamReader sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            // Jeżeli pierwszy obiekt
            content = content.TrimEnd();
            content = content.Remove(content.Length - 1, 1);
            // Pierwszy obiekt, nie dodawaj przecinka przed
            if (content.Length == 1)
            {
                content = String.Format("{0}\n{1}\n]\n", content.Trim(), newJsonObject);
            }
            else
            {
                content = String.Format("{0},\n{1}\n]\n", content.Trim(), newJsonObject);
            }

            using (StreamWriter sw = File.CreateText(filename))
            {
                sw.Write(content);
            }
        }
    }
}

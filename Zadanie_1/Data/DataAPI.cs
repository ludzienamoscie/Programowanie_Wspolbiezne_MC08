using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("DataTest")]

namespace Data
{
    internal class DataAPI : DataAbstractAPI
    {
        private ObservableCollection<Ball> _ballList;
        // Zmienna odpowiadajaca za usuwanie pliku az kazdym utworzeniem nowego zestawu kul
        private bool _newSession = true;
        public override double RectWidth => 785;
        public override double RectHeight => 320;
        public override double Stroke => 10;
        public override double MassLimit => 100;
        public override bool NewSession
        {
            get => _newSession;
            set => _newSession = value;
        }
        public override ObservableCollection<Ball> CreateBalls(int number, double XLimit, double YLimit, double Stroke, double MLimit)
        {
            _ballList = new ObservableCollection<Ball>();
            NewSession = true;
            double x, y, r, m, vx, vy;
            double speed = 2;
            Random random = new Random();
            for (int i = 0; i < number; i++)
            {
                Thread.Sleep(1);
                r = 20;
                x = random.Next((int)Stroke, (int)(XLimit - r) - 1) + random.NextDouble();
                y = random.Next((int)Stroke, (int)(YLimit - r) - 1) + random.NextDouble();
                m = random.Next(1, (int)MLimit - 1) + random.NextDouble();
                vx = random.NextDouble() * speed * (random.Next(0, 1) == 0 ? -1 : 1);
                vy = Math.Sqrt(speed * speed - vx * vx) * (random.Next(0, 1) == 0 ? -1 : 1);
                _ballList.Add(new Ball(x, y, r, m, vx, vy));
            }
            return _ballList;
        }
        public override void AppendObjectToJSONFile(string filename, string newJsonObject)
        {
            // Jeżeli plik istnieje i jest nowy zestaw kul, to usuń stary plik
            if (File.Exists(filename) && NewSession)
            {
                NewSession = false;
                File.Delete(filename);
            }

            using (StreamWriter sw = new StreamWriter(filename, true))
            {
                sw.WriteLine("[]");
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

        public override CollisionInfo CollisionInfoObject(Vector2D initial_vel_1, Vector2D initial_vel_2, Ball ball_1, Ball ball_2)
        {
            return new CollisionInfo(initial_vel_1, initial_vel_2, ball_1, ball_2);
        }
    }
}

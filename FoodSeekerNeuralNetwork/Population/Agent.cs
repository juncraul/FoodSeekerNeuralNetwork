using Mathematics;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Population
{
    public class Agent : BasePopulation
    {
        public double Energy { get; set; }

        private readonly int _eyesCount;
        private Network _network;
        private double _distanceBetweenEyes;
        private double _eyesRadius;
        private double _eyeLength;
        private BasePopulation[] _eyeSees;

        public Agent(Vector2 position, int eyesCount)
        {
            Energy = 100;
            Position = position;
            Radius = 20;
            Color = Color.Red;
            _eyesCount = eyesCount;
            _eyeSees = new BasePopulation[_eyesCount];
            _eyeLength = 30;
            _eyesRadius = Math.PI / 2;//90 degree
            _distanceBetweenEyes = _eyesRadius / _eyesCount;
            _network = new Network();
            _network.InitializeNetwork(_eyesCount, 5, 3, 0.3f);
        }

        public void CheckEyes(List<BasePopulation> items)
        {
            for(int i = 0; i < _eyesCount; i ++)
            {
                double distance;
                double minDistance = 1000000;
                foreach (BasePopulation b in items)
                {
                    if (b == this) continue;
                    Vector2 direction = (new Vector2(-_eyeLength, 0).Rotate(-(Math.PI / 2 - _eyesRadius / 2 + (i * _distanceBetweenEyes))));
                    if (Functions.RayIntersectsCricle(Position, direction, b.Position, b.Radius, out distance))
                    {
                        if(minDistance > distance)
                        {
                            minDistance = distance;
                            _eyeSees[i] = b;
                        }
                    }
                }
                if(minDistance == 1000000)
                {
                    _eyeSees[i] = null;
                }

            }
        }

        public void SendSignals(double foodSignal0, double foodSignal1)
        {
            //TODO: needs unsupervised learning
            //_network.TrainNetwrok()
        }

        public void Draw(Graphics graphics, Bitmap bitmap)
        {
            SolidBrush brush = new SolidBrush(Color);
            graphics.FillEllipse(brush, new Rectangle((int)(Position.X - Radius / 2), bitmap.Height - (int)(Position.Y + Radius/ 2), (int)Radius, (int)Radius));

            for (int i = 0; i < _eyesCount; i ++)
            {
                Pen pen = new Pen(_eyeSees[i] != null ? _eyeSees[i].Color : Color.Black);
                Vector2 direction = (new Vector2(-_eyeLength, 0).Rotate(-(Math.PI / 2 - _eyesRadius / 2 + (i * _distanceBetweenEyes))));
                graphics.DrawLine(pen, (int)Position.X, bitmap.Height - (int)Position.Y, (int)(Position + direction).X, bitmap.Height - (int)(Position + direction).Y);
            }

            string text = "E:" + Energy;

            brush.Color = Color.Black;
            graphics.DrawString(text, new Font("Consolas", 8), brush, (int)Position.X + 4, (int)(bitmap.Height - Position.Y));
        }

        public class AgentComparer : IEqualityComparer<BasePopulation>
        {

            public bool Equals(BasePopulation x, BasePopulation y)
            {
                //Check whether the objects are the same object. 
                return Object.ReferenceEquals(x, y);
            }

            public int GetHashCode(BasePopulation obj)
            {
                //Get hash code for the Name field if it is not null. 
                int hashPosition = obj.Position == null ? 0 : obj.Position.GetHashCode();

                //Get hash code for the Code field. 
                int hashRadius = obj.Radius.GetHashCode();

                int hashColor = obj.Color.GetHashCode();

                //Calculate the hash code for the product. 
                return hashPosition ^ hashRadius ^ hashColor;
            }
        }
    }
}

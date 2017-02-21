using Mathematics;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Population
{
    public class Agent : BasePopulation
    {
        public double Health { get; set; }

        private readonly int _eyesCount;
        private Network _network;
        private double _distanceBetweenEyes;
        private double _eyesRadius;
        private double _eyeLength;
        private BasePopulation[] _eyeSees;

        public Agent(Vector2 position, int eyesCount)
        {
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
                bool eyeSeesSomething = false;
                foreach(BasePopulation b in items)
                {
                    if (b == this) continue;
                    if(Functions.RayIntersectsCricle(Position, Position + new Vector2(-1, 0).Rotate((Math.PI / 2 - _eyesRadius / 2 + i * _distanceBetweenEyes)), b.Position, b.Radius))
                    {
                        _eyeSees[i] = b;//TODO:check if there is a closer one
                        eyeSeesSomething = true;
                        break;
                    }
                }
                if(!eyeSeesSomething)
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
                Vector2 direction = Position + (new Vector2(-1, 0).Rotate(-(Math.PI / 2 - _eyesRadius / 2 + (i * _distanceBetweenEyes))).Normaize() * _eyeLength);
                graphics.DrawLine(pen, (int)Position.X, bitmap.Height - (int)Position.Y, (int)direction.X, bitmap.Height - (int)direction.Y);
            }
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

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
        private double _directionRadian;
        private double _rotationSpeed;
        private double _thrustSpeed;
        private double _speed;

        private Matrix previousSensorMatrix;

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
            _rotationSpeed = 3.6f;
            _thrustSpeed = 3;
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
                    Vector2 direction = (new Vector2(-_eyeLength, 0).Rotate(_directionRadian - (Math.PI / 2 - _eyesRadius / 2 + (i * _distanceBetweenEyes))));
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

        public void SendSignalsToBrain()
        {
            bool hasFoodInFront = false;
            Matrix sensorMatrix = new Matrix(_eyesCount, 1);
            for(int i = 0; i < _eyesCount; i ++)
            {
                if (_eyeSees[i] == null) continue;
                if(_eyeSees[i].Color == Color.Red)
                {
                    sensorMatrix.TheMatrix[i, 0] = 0.3f;
                } else if(_eyeSees[i].Color == Color.GreenYellow)
                {
                    sensorMatrix.TheMatrix[i, 0] = 0.6f;
                    hasFoodInFront = true;
                }
                else if (_eyeSees[i].Color == Color.Brown)
                {
                    sensorMatrix.TheMatrix[i, 0] = 0.9f;
                }
            }

            Matrix expectedResonse = GetExpectedOutput(hasFoodInFront);

            Matrix actualResponse = _network.TrainNetwrok(sensorMatrix, expectedResonse);
            previousSensorMatrix = sensorMatrix;

            _directionRadian += actualResponse.TheMatrix[0, 0] * _rotationSpeed;
            _directionRadian -= actualResponse.TheMatrix[1, 0] * _rotationSpeed;
            _speed = actualResponse.TheMatrix[2, 0] * _thrustSpeed;

            //if(brainResponse.TheMatrix[0, 0] > brainResponse.TheMatrix[1, 0])
            //    Color = Color.Chocolate;

            //if (actualResponse.TheMatrix[2, 0] < 0)
            //    Color = Color.Chocolate;
        }

        public void AgentActivity()
        {
            Position += new Vector2(0, 1).Rotate(_directionRadian) * _speed;
            Energy-= 0.1;
        }

        public Matrix GetExpectedOutput(bool hasFoodInFront)
        {
            Matrix expectedOutput = new Matrix(3, 1);

            if(hasFoodInFront)
            {
                expectedOutput.TheMatrix[0, 0] = 0;
                expectedOutput.TheMatrix[1, 0] = 0;
                expectedOutput.TheMatrix[2, 0] = 1;
            }
            else
            {
                expectedOutput.TheMatrix[0, 0] = 1;
                expectedOutput.TheMatrix[1, 0] = 1;
                expectedOutput.TheMatrix[2, 0] = 0;
            }

            return expectedOutput;
        }

        public void CheckForFood(List<Food> food)
        {
            for(int i = food.Count - 1; i >= 0; i --)
            {
                if(Functions.CirclesCollision(Position, Radius, food[i].Position, food[i].Radius))
                {
                    Energy += food[i].FoodValue;
                    food.RemoveAt(i);
                }

            }
        }

        public void Draw(Graphics graphics, Bitmap bitmap)
        {
            SolidBrush brush = new SolidBrush(Color);
            Pen pen = new Pen(Color.Black);
            pen.Width = 2;
            graphics.FillEllipse(brush, new Rectangle((int)(Position.X - Radius), bitmap.Height - (int)(Position.Y + Radius), (int)Radius * 2, (int)Radius * 2));
            graphics.DrawEllipse(pen, new Rectangle((int)(Position.X - Radius), bitmap.Height - (int)(Position.Y + Radius), (int)Radius * 2, (int)Radius * 2));

            pen.Width = 1;
            for (int i = 0; i < _eyesCount; i ++)
            {
                pen.Color = _eyeSees[i] != null ? _eyeSees[i].Color : Color.Black;
                Vector2 direction = (new Vector2(-_eyeLength, 0).Rotate(_directionRadian - (Math.PI / 2 - _eyesRadius / 2 + (i * _distanceBetweenEyes))));
                graphics.DrawLine(pen, (int)Position.X, bitmap.Height - (int)Position.Y, (int)(Position + direction).X, bitmap.Height - (int)(Position + direction).Y);
            }

            string text = "E:" + (int)Energy;

            brush.Color = Color.Black;
            graphics.DrawString(text, new Font("Consolas", 8), brush, (int)Position.X + 20, (int)(bitmap.Height - Position.Y));
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

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
        public bool IsSelected { get; set; }
        public int FoodAte { get; set; }

        public readonly List<string> EatsOtherSpecies;

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
        private double _energyDecay;
        private double _maxEnergy;

        private Matrix previousSensorMatrix;

        private int _tempA;
        private int _tempB;

        public Agent(Vector2 position, int eyesCount, string specieType, Color color, List<string> eatsOtherSpecies, Random rand)
        {
            _maxEnergy = 1000;
            Energy = 100;
            Position = position;
            Radius = 10;
            _energyDecay = 0.15;
            Color = color;
            _eyesCount = eyesCount;
            _eyeSees = new BasePopulation[_eyesCount];
            _eyeLength = 20;
            _eyesRadius = Math.PI / 2;//90 degree
            _distanceBetweenEyes = _eyesRadius / _eyesCount;
            _rotationSpeed = 0.2f;
            _thrustSpeed = 6;
            _network = new Network();
            _network.InitializeNetwork(_eyesCount + 1, 15, 2, 0.3f, rand);
            SpecieType = specieType;
            EatsOtherSpecies = eatsOtherSpecies;
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

        public void TrainTheAgent()
        {
            Matrix sensorMatrix = new Matrix(_eyesCount + 1, 1);
            for(int i = 0; i < _eyesCount; i ++)
            {
                if (_eyeSees[i] == null) continue;
                int colourValue = _eyeSees[i].Color.R * 255 * 255 + _eyeSees[i].Color.G * 255 + _eyeSees[i].Color.B;
                sensorMatrix.TheMatrix[i, 0] = colourValue / (256.0 * 256.0 * 256.0) * 0.98 + 0.01;
            }

            sensorMatrix.TheMatrix[_eyesCount, 0] = Energy / _maxEnergy;

            Matrix actualResponse;
            actualResponse = _network.QueryNetwrok(sensorMatrix);
            previousSensorMatrix = sensorMatrix;

            _directionRadian += (actualResponse.TheMatrix[0, 0] * 2 - 1) * _rotationSpeed;
            _speed = (actualResponse.TheMatrix[1, 0] //* 2 - 1
                ) * _thrustSpeed;
            _tempB++;
        }

        public void AgentActivity()
        {
            Position += new Vector2(0, 1).Rotate(_directionRadian) * _speed;
            Energy-= _energyDecay;
            _energyDecay += 0.001;
            _tempA++;
        }

        public void CheckForFood(List<Food> food, List<Agent> agents)
        {
            for(int i = food.Count - 1; i >= 0; i --)
            {
                if (!EatsOtherSpecies.Contains(food[i].SpecieType))
                    continue;
                if (Functions.CirclesCollision(Position, Radius, food[i].Position, food[i].Radius))
                {
                    FoodAte++;
                    Energy += food[i].FoodValue;
                    food[i].IsAlive = false;
                }
            }

            for (int i = agents.Count - 1; i >= 0; i--)
            {
                if (agents[i] == this)
                    continue;
                if (!EatsOtherSpecies.Contains(agents[i].SpecieType))
                    continue;
                if (Functions.CirclesCollision(Position, Radius, agents[i].Position, agents[i].Radius))
                {
                    FoodAte++;
                    Energy += agents[i].Energy / 2;
                    agents[i].IsAlive = false;
                }
            }
            if (Energy > _maxEnergy)
                Energy = _maxEnergy;
        }

        public void DrawAgent(Graphics graphics, Bitmap bitmap)
        {
            SolidBrush brush = new SolidBrush(Color);
            Pen pen = new Pen(Color.Black);
            pen.Width = 2;
            graphics.FillEllipse(brush, new Rectangle((int)(Position.X - Radius), bitmap.Height - (int)(Position.Y + Radius), (int)Radius * 2, (int)Radius * 2));
            graphics.DrawEllipse(pen, new Rectangle((int)(Position.X - Radius), bitmap.Height - (int)(Position.Y + Radius), (int)Radius * 2, (int)Radius * 2));

            if(IsSelected)
                graphics.DrawRectangle(pen, new Rectangle((int)(Position.X - Radius), bitmap.Height - (int)(Position.Y + Radius), (int)Radius * 2, (int)Radius * 2));

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

        public void DrawBrain(Graphics graphics, Bitmap bitmap)
        {
            _network.Draw(graphics, bitmap);
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

        public string GetBrainAsBits()
        {
            return _network.ConvertNetworkToBitString();
        }

        public void InsertNewBrainAsBits(string bits)
        {
            _network.ConvertBitStringToNetwork(bits);
        }
    }
}

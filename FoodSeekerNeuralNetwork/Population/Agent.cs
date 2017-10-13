using Mathematics;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Population
{
    public enum AgentSettings
    {
        None = 0,
        DontDecay = 1,
        ShowFoodAte = 2,
        ShowEnergy = 4,
    }

    public class Agent : BasePopulation
    {
        public double Energy { get; set; }
        public bool IsSelected { get; set; }
        public int GoodFoodAte { get; set; }

        public int BadFoodAte { get; set; }

        public readonly List<SpecieType> EatsOtherSpecies;

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
        private AgentSettings _agentSettings;

        private Food _closestFood;


        public Agent(Vector2 position, int eyesCount, int hiddenNeurons, SpecieType agentType, Color color, List<SpecieType> eatsOtherSpecies, Random rand, AgentSettings agentSettings = AgentSettings.None)
        {
            _maxEnergy = 100;
            Energy = 100;
            Position = position;
            Radius = 10;
            _energyDecay = 0.35;
            Color = color;
            _eyesCount = eyesCount;
            _eyeSees = new BasePopulation[_eyesCount];
            _eyeLength = 20;
            _eyesRadius = Math.PI /2;
            _distanceBetweenEyes = _eyesRadius / _eyesCount;
            _rotationSpeed = 0.2f;
            _thrustSpeed = 3;
            _network = new Network();
            _network.InitializeNetwork(_eyesCount * 2 + 1, hiddenNeurons, 1, 0.3f, rand);
            //_network.InitializeNetwork(_eyesCount + 1, hiddenNeurons, 1, 0.3f, rand);
            Type = agentType;
            EatsOtherSpecies = eatsOtherSpecies;
            _agentSettings = agentSettings;
        }

        public void CheckEyes(List<BasePopulation> items)
        {
            double distance;
            double minDistance;
            for(int i = 0; i < _eyesCount; i ++)
            {
                minDistance = 1000000;
                foreach (BasePopulation b in items)
                {
                    if (b == this) continue;
                    if ((b as Agent) != null) continue;
                    Vector2 direction = (new Vector2(_eyeLength, 0).Rotate(_directionRadian - _eyesRadius / 2 + (i * _distanceBetweenEyes)));
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

            minDistance = 1000000;
            foreach(BasePopulation b in items)
            {
                if (b == this) continue;
                if ((b as Food) == null) continue;
                distance = Functions.DistanceBetweenTwoPoints(b.Position, Position);
                if (distance < minDistance)
                {
                    _closestFood = b as Food;
                    minDistance = distance;
                }
            }
        }

        public void TrainTheAgent()
        {
            Matrix sensorMatrix = new Matrix(_eyesCount * 2 + 1, 1);
            //Matrix sensorMatrix = new Matrix(_eyesCount + 1, 1);
            for (int i = 0; i < _eyesCount; i ++)
            {
                if (_eyeSees[i] != null)
                {
                    //int colourValue = _eyeSees[i].Color.R * 255 * 255 + _eyeSees[i].Color.G * 255 + _eyeSees[i].Color.B;
                    sensorMatrix.TheMatrix[i * 2, 0] = _eyeSees[i].Color == Color.Gray ? 0.5 : 0.9; // (256.0 * 256.0 * 256.0) * 0.98 + 0.01;
                    sensorMatrix.TheMatrix[i * 2 + 1, 0] = 1 - Functions.DistanceBetweenTwoPoints(_eyeSees[i].Position, Position) / 10000.0;
                    //sensorMatrix.TheMatrix[i, 0] = 0.9;
                }
                else
                {
                    sensorMatrix.TheMatrix[i * 2, 0] = 0.01;
                    sensorMatrix.TheMatrix[i * 2 + 1, 0] = 0.01;
                    //sensorMatrix.TheMatrix[i, 0] = 0.01;
                }
            }
            //sensorMatrix.TheMatrix[0, 0] = (Functions.AngleBetweenTwoPoints(Position, _closestFood.Position) + 0.001) / (Math.PI * 2 + 0.002);
            //sensorMatrix.TheMatrix[1, 0] = Functions.DistanceBetweenTwoPoints(Position, _closestFood.Position) / 1000;
            //sensorMatrix.TheMatrix[2, 0] = (_directionRadian + 0.001) / (Math.PI * 2 + 0.002);

            sensorMatrix.TheMatrix[_eyesCount * 2, 0] = Energy / _maxEnergy;
            //sensorMatrix.TheMatrix[_eyesCount, 0] = Energy / _maxEnergy;

            Matrix actualOutput;
            actualOutput = _network.QueryNetwrok(sensorMatrix);
            previousSensorMatrix = sensorMatrix;

            _directionRadian += (actualOutput.TheMatrix[0, 0] * 2 - 1) * _rotationSpeed;
            _speed = //(actualResponse.TheMatrix[1, 0] //* 2 - 1
                //) * 
                _thrustSpeed;
            _directionRadian = (_directionRadian > 0 ? (_directionRadian * 100) % 628 : 628 - (-_directionRadian * 100) % 628) / 100.0;
        }

        public void AgentActivity()
        {
            Position += new Vector2(1, 0).Rotate(_directionRadian) * _speed;
            if(!_agentSettings.HasFlag(AgentSettings.DontDecay))
            {
                Energy-= _energyDecay;
                if (Energy <= 0)
                    IsAlive = false;
            }
        }

        public void CheckForFood(List<Food> food, List<Agent> agents)
        {
            for(int i = food.Count - 1; i >= 0; i --)
            {
                if (!EatsOtherSpecies.Contains(food[i].Type))
                    continue;
                if (Functions.CirclesCollision(Position, Radius, food[i].Position, food[i].Radius))
                {
                    if (food[i].FoodValue > 0)
                    {
                        GoodFoodAte++;
                    }
                    else
                    {
                        BadFoodAte++;
                    }

                    Energy += food[i].FoodValue;
                    food[i].IsAlive = false;
                }
            }

            for (int i = agents.Count - 1; i >= 0; i--)
            {
                if (agents[i] == this)
                    continue;
                if (!EatsOtherSpecies.Contains(agents[i].Type))
                    continue;
                if (Functions.CirclesCollision(Position, Radius, agents[i].Position, agents[i].Radius))
                {
                    GoodFoodAte++;
                    Energy += agents[i]._maxEnergy / 2;
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
                Vector2 direction = (new Vector2(_eyeLength, 0).Rotate(_directionRadian - _eyesRadius / 2 + (i * _distanceBetweenEyes)));
                graphics.DrawLine(pen, (int)Position.X, bitmap.Height - (int)Position.Y, (int)(Position + direction).X, bitmap.Height - (int)(Position + direction).Y);
            }

            string text = string.Empty;
            if (_agentSettings.HasFlag(AgentSettings.ShowFoodAte))
            {
                text = "Ate:" + GoodFoodAte + Environment.NewLine;
            }
            if (_agentSettings.HasFlag(AgentSettings.ShowEnergy))
            {
                text = "E:" + (int)Energy + Environment.NewLine;
            }

            brush.Color = Color.Black;
            graphics.DrawString(text, new Font("Consolas", 8), brush, (int)Position.X + 20, (int)(bitmap.Height - Position.Y));
        }

        public void DrawBrain(Graphics graphics, Bitmap bitmap)
        {
            _network.Draw(graphics, bitmap);
        }

        public double GetFitness(double scoreForEatingGoodFood, double scoreForEatingBadFood, double scoreForExisting)
        {
            double fitness = GoodFoodAte * scoreForEatingGoodFood + BadFoodAte * scoreForEatingBadFood + scoreForExisting + (IsAlive ? 100 : - 100);
            return fitness > 0 ? fitness : 1;
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
            //return _network.ConvertNetworkToBitString();
            return _network.ConvertNetworkToBitStringHiddenLayerOrder();
        }

        public void InsertNewBrainAsBits(string bits)
        {
            //_network.ConvertBitStringToNetwork(bits);
            _network.ConvertBitStringToNetworkHiddenLayerOrder(bits);
        }

        public int GetHiddenNeurons()
        {
            return _network.HiddenNodes;
        }
    }
}

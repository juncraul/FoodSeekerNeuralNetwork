using Mathematics;
using Population;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GeneticProgramming;

namespace FoodSeekerNeuralNetwork
{
    public class ApplicationEngine
    {
        private static ApplicationEngine _applicationEngineInstance;
        private static GeneticEvolution _geneticEvolution;
        public List<Agent> agents;
        public List<Food> food;
        Bitmap bitmapWorld;
        Graphics graphicsWorld;

        Bitmap bitmapBrain;
        Graphics graphicsBrain;
        
        Bitmap bitmapGraph;
        Graphics graphicsGraph;

        Graph evolutionGraph;

        DateTime timeWhenApplicationStarted;
        DateTime lastTimeFoodWasGenerated;

        int numberOfAgentsTypeOne = 15;
        int numberOfAgentsTypeTwo = 0;
        int numberOfEyes = 16;
        List<string> types = new List<string>();
        Color colorZero = Color.GreenYellow;
        List<string> typeOneEats = new List<string>();
        List<string> typeTwoEats = new List<string>();
        List<Color> _agentsColor;
        int ticksForGeneration;
        int ticksIntoGeneration;
        double fitnessThisGenerationPrey;
        double fitnessThisGenerationPredator;

        private ApplicationEngine(Size worldCanvasSize, Size brainCanvasSize, Size graphCanvasSize)
        {
            ticksIntoGeneration = 0;
            ticksForGeneration = 10;
            timeWhenApplicationStarted = DateTime.Now;
            _geneticEvolution = new GeneticEvolution(ApplicationSettings.Random, ApplicationSettings.MutationRate, ApplicationSettings.CrossOverRate);
            _agentsColor = new List<Color>();
            agents = new List<Agent>();
            food = new List<Food>();
            bitmapWorld = new Bitmap(worldCanvasSize.Width, worldCanvasSize.Height);
            graphicsWorld = Graphics.FromImage(bitmapWorld);

            bitmapBrain = new Bitmap(brainCanvasSize.Width, brainCanvasSize.Height);
            graphicsBrain = Graphics.FromImage(bitmapBrain);

            bitmapGraph = new Bitmap(graphCanvasSize.Width, graphCanvasSize.Height);
            graphicsGraph = Graphics.FromImage(bitmapGraph);

            types = new List<string>
            {
                "Zero",
                "One",
                "Two"
            };
            typeOneEats.Add(types[0]);
            typeTwoEats.Add(types[1]);
            _agentsColor.Add(Color.Red);
            _agentsColor.Add(Color.Blue);

            for (int i = 0; i < numberOfAgentsTypeOne; i++)
            {
                GenerateAgent(1, false);
            }

            for (int i = 0; i < numberOfAgentsTypeTwo; i++)
            {
                GenerateAgent(2, false);
            }

            for (int i = 0; i < 10; i++)
            {
                GenerateFood();
            }

            evolutionGraph = new Graph(200, 500, 2, _agentsColor);
        }

        public static ApplicationEngine GetInstance(Size worldCanvasSize, Size brainCanvasSize, Size graphCanvasSize)
        {
            return _applicationEngineInstance = (_applicationEngineInstance ?? new ApplicationEngine(worldCanvasSize, brainCanvasSize, graphCanvasSize));
        }

        public Bitmap DrawWorld()
        {
            SolidBrush brush = new SolidBrush(Color.White);
            graphicsWorld.FillRectangle(brush, new Rectangle(0, 0, bitmapWorld.Width, bitmapWorld.Height));
            foreach (Food f in food)
            {
                f.Draw(graphicsWorld, bitmapWorld);
            }
            foreach (Agent a in agents)
            {
                a.DrawAgent(graphicsWorld, bitmapWorld);
            }

            return bitmapWorld;
        }

        public Bitmap DrawBrain()
        {
            SolidBrush brush = new SolidBrush(Color.White);
            graphicsBrain.FillRectangle(brush, new Rectangle(0, 0, bitmapWorld.Width, bitmapWorld.Height));
            Agent agent = agents.Where(a =>a.IsSelected).FirstOrDefault();
            if (agent == null)
                agent = agents.FirstOrDefault();
            agent.DrawBrain(graphicsBrain, bitmapBrain);

            return bitmapBrain;
        }

        public Bitmap DrawGraph()
        {
            SolidBrush brush = new SolidBrush(Color.White);
            graphicsGraph.FillRectangle(brush, new Rectangle(0, 0, bitmapGraph.Width, bitmapGraph.Height));
            
            evolutionGraph.Draw(graphicsGraph, bitmapGraph);

            return bitmapGraph;
        }

        public void DoLogic()
        {
            fitnessThisGenerationPrey += agents.Where(a => a.SpecieType == types[1]).Sum(a => a.Energy);
            fitnessThisGenerationPredator += agents.Where(a => a.SpecieType == types[2]).Sum(a => a.Energy);
            ticksIntoGeneration++;
            if (ticksIntoGeneration == ticksForGeneration)
            {
                ticksIntoGeneration = 0;
                evolutionGraph.AddPoint(0, fitnessThisGenerationPrey / ticksForGeneration / numberOfAgentsTypeOne);
                evolutionGraph.AddPoint(1, agents.Where(a => a.SpecieType == types[1]).Sum(a => a.FoodAte));
                fitnessThisGenerationPrey = 0;
                fitnessThisGenerationPredator = 0;
            }

            List<BasePopulation> allPopulation = agents.Cast<BasePopulation>().Union(food.Cast<BasePopulation>()).ToList();

            for (int i = agents.Count - 1; i >= 0; i--)
            {
                if (!agents[i].IsAlive)
                    continue;
                agents[i].CheckEyes(allPopulation);
                agents[i].TrainTheAgent();
                agents[i].AgentActivity();
                agents[i].CheckForFood(food, agents);
            }

            ReplaceDeadPopulation();

            TimeSpan timeBetweenFoodGeneration = (DateTime.Now - lastTimeFoodWasGenerated);
            if (timeBetweenFoodGeneration.TotalMilliseconds > ApplicationSettings.GenerateFoodMilliseconds)
            {
                GenerateFood();
            }
        }

        private void ReplaceDeadPopulation()
        {
            agents.RemoveAll(a => a.Energy <= 0 || !a.IsAlive);
            food.RemoveAll(f => !f.IsAlive);

            for (int i = agents.Count; i < numberOfAgentsTypeOne + numberOfAgentsTypeTwo; i++)
            {
                if (agents.Count(a => a.SpecieType == types[1]) != numberOfAgentsTypeOne)
                {
                    GenerateAgent(1, true);
                }
                else
                {
                    GenerateAgent(2, true);
                }
            }
        }

        public void GenerateAgent(int agentId, bool isCrossoverAgent)
        {
            Agent agent = new Agent(new Vector2(ApplicationSettings.Random.Next() % bitmapWorld.Width, ApplicationSettings.Random.Next() % bitmapWorld.Height), 
                                                numberOfEyes, types[agentId], _agentsColor[agentId - 1], agentId == 1 ? typeOneEats : typeTwoEats, ApplicationSettings.Random);
            if (isCrossoverAgent)
            {
                if (agents.Count(a => a.SpecieType == types[agentId]) <= 1)
                {
                    GenerateAgent(agentId, false);
                    return;
                }
                int indexParent0 = _geneticEvolution.Roulette(agents.Where(a => a.SpecieType == types[agentId])
                                                                    .Select(a=>a.Energy + a.FoodAte * ApplicationSettings.ScoreForEatingFood).ToArray());
                int indexParent1 = _geneticEvolution.Roulette(agents.Where(a => a.SpecieType == types[agentId])
                                                                    .Select(a => a.Energy + a.FoodAte * ApplicationSettings.ScoreForEatingFood).ToArray());
                string networkAsBits = _geneticEvolution.Reproduce(agents[indexParent0].GetBrainAsBits(),
                                                            agents[indexParent1].GetBrainAsBits());
                agent.InsertNewBrainAsBits(networkAsBits);
            }
            agents.Add(agent);
        }

        public void GenerateFood()
        {
            lastTimeFoodWasGenerated = DateTime.Now;
            if (food.Count > agents.Count * 2.5)
                return;
            food.Add(new Food(new Vector2(ApplicationSettings.Random.Next() % bitmapWorld.Width, ApplicationSettings.Random.Next() % bitmapWorld.Height), types[0], colorZero));
        }

        public void SelectAgent(Point p)
        {
            foreach (Agent a in agents)
            {
                a.IsSelected = false;
            }

            foreach (Agent a in agents)
            {
                if(Functions.CollisionPointCircle(new Vector2(p.X, p.Y), a.Position, a.Radius))
                {
                    a.IsSelected = true;
                    break;
                }
            }
        }
    }
}

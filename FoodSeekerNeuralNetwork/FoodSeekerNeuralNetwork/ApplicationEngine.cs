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
        public List<Food> badFood;
        Bitmap bitmapWorld;
        Graphics graphicsWorld;

        Bitmap bitmapBrain;
        Graphics graphicsBrain;
        
        Bitmap bitmapGraph;
        Graphics graphicsGraph;

        Graph evolutionGraph;

        DateTime timeWhenApplicationStarted;
        DateTime lastTimeFoodWasGenerated;

        
        int numberOfEyes = 16;
        Color colorZero = Color.GreenYellow;
        List<SpecieType> typeOneEats = new List<SpecieType>();
        List<SpecieType> typeTwoEats = new List<SpecieType>();
        List<Color> _agentsColor;
        Color colorFood = Color.GreenYellow;
        Color colorBadFood = Color.Gray;
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
            badFood = new List<Food>();
             bitmapWorld = new Bitmap(worldCanvasSize.Width, worldCanvasSize.Height);
            graphicsWorld = Graphics.FromImage(bitmapWorld);

            bitmapBrain = new Bitmap(brainCanvasSize.Width, brainCanvasSize.Height);
            graphicsBrain = Graphics.FromImage(bitmapBrain);

            bitmapGraph = new Bitmap(graphCanvasSize.Width, graphCanvasSize.Height);
            graphicsGraph = Graphics.FromImage(bitmapGraph);
            
            typeOneEats.Add(SpecieType.Plant);
            typeTwoEats.Add(SpecieType.Herbivore);
            _agentsColor.Add(Color.Red);
            _agentsColor.Add(Color.Blue);

            for (int i = 0; i < ApplicationSettings.NumberOfAgentsTypeOne; i++)
            {
                GenerateAgent(SpecieType.Herbivore, false);
            }

            for (int i = 0; i < ApplicationSettings.NumberOfAgentsTypeTwo; i++)
            {
                GenerateAgent(SpecieType.Carnivore, false);
            }

            for (int i = 0; i < ApplicationSettings.FoodOnScreen; i++)
            {
                GenerateFood();
            }

            for (int i = 0; i < ApplicationSettings.BadFoodOnScreen; i++)
            {
                GenerateBadFood();
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
            foreach (Food f in badFood)
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
            fitnessThisGenerationPrey += agents.Where(a => a.Type == SpecieType.Herbivore).Sum(a => a.GetFitness(ApplicationSettings.ScoreForEatingGoodFood, ApplicationSettings.ScoreForEatingBadFood, ApplicationSettings.ScoreForExisting));
            fitnessThisGenerationPredator += agents.Where(a => a.Type == SpecieType.Carnivore).Sum(a => a.Energy);
            ticksIntoGeneration++;
            if (ticksIntoGeneration == ticksForGeneration)
            {
                ticksIntoGeneration = 0;
                evolutionGraph.AddPoint(0, fitnessThisGenerationPrey / ticksForGeneration / ApplicationSettings.NumberOfAgentsTypeOne);
                evolutionGraph.AddPoint(1, agents.Where(a => a.Type == SpecieType.Herbivore).Sum(a => a.GoodFoodAte));
                fitnessThisGenerationPrey = 0;
                fitnessThisGenerationPredator = 0;
            }

            List<BasePopulation> allPopulation = agents.Cast<BasePopulation>()
                                                    .Union(food.Cast<BasePopulation>())
                                                    .Union(badFood.Cast<BasePopulation>()).ToList();

            for (int i = agents.Count - 1; i >= 0; i--)
            {
                if (!agents[i].IsAlive)
                    continue;
                agents[i].CheckEyes(allPopulation);
                agents[i].TrainTheAgent();
                agents[i].AgentActivity();
                agents[i].CheckForFood(food.Union(badFood.Cast<Food>()).ToList(), agents);
                food.RemoveAll(f => !f.IsAlive);
                badFood.RemoveAll(f => !f.IsAlive);
            }

            ReplaceDeadPopulation();

            for (int i = food.Count; i < ApplicationSettings.FoodOnScreen; i++)
            {
                GenerateFood();
            }

            for (int i = badFood.Count; i < ApplicationSettings.BadFoodOnScreen; i++)
            {
                GenerateBadFood();
            }
        }

        private void ReplaceDeadPopulation()
        {
            agents.RemoveAll(a => a.Energy <= 0 || !a.IsAlive);
            food.RemoveAll(f => !f.IsAlive);

            for (int i = agents.Count; i < ApplicationSettings.NumberOfAgentsTypeOne + ApplicationSettings.NumberOfAgentsTypeTwo; i++)
            {
                if (agents.Count(a => a.Type == SpecieType.Herbivore) != ApplicationSettings.NumberOfAgentsTypeOne)
                {
                    GenerateAgent(SpecieType.Herbivore, true);
                }
                else
                {
                    GenerateAgent(SpecieType.Carnivore, true);
                }
            }
        }

        public void GenerateAgent(SpecieType agentType, bool isCrossoverAgent)
        {
            Agent agent;

            if (isCrossoverAgent)
            {
                if (agents.Count(a => a.Type == agentType) <= 1)
                {
                    GenerateAgent(agentType, false);
                    return;
                }
                int indexParent0 = _geneticEvolution.Roulette(agents.Where(a => a.Type == agentType)
                                                                    .Select(a=> a.GetFitness(ApplicationSettings.ScoreForEatingGoodFood, ApplicationSettings.ScoreForEatingBadFood, ApplicationSettings.ScoreForExisting)).ToArray());
                int indexParent1 = _geneticEvolution.Roulette(agents.Where(a => a.Type == agentType)
                                                                    .Select(a => a.GetFitness(ApplicationSettings.ScoreForEatingGoodFood, ApplicationSettings.ScoreForEatingBadFood, ApplicationSettings.ScoreForExisting)).ToArray());
                string brainOfAgent0 = agents[indexParent0].GetBrainAsBits();
                string brainOfAgent1 = agents[indexParent1].GetBrainAsBits();

                string smaller = brainOfAgent0.Length > brainOfAgent1.Length ? brainOfAgent0 : brainOfAgent1;
                string bigger = brainOfAgent0.Length < brainOfAgent1.Length ? brainOfAgent0 : brainOfAgent1;

                int hiddenNeurons;

                if (ApplicationSettings.Random.Next() % 2 == 0)
                {//append it
                    smaller = smaller + bigger.Substring(smaller.Length);
                    hiddenNeurons = brainOfAgent0.Length > brainOfAgent1.Length ? agents[indexParent0].GetHiddenNeurons() : agents[indexParent1].GetHiddenNeurons();
                }
                else
                {//cut it
                    bigger = bigger.Substring(0, smaller.Length);
                    hiddenNeurons = brainOfAgent0.Length < brainOfAgent1.Length ? agents[indexParent0].GetHiddenNeurons() : agents[indexParent1].GetHiddenNeurons();
                }
                brainOfAgent0 = smaller;
                brainOfAgent1 = bigger;

                string networkAsBits = _geneticEvolution.Reproduce(brainOfAgent0, brainOfAgent1);

                agent = new Agent(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height),
                                                numberOfEyes, hiddenNeurons, agentType, _agentsColor[(int)agentType], agentType == SpecieType.Herbivore ? typeOneEats : typeTwoEats, ApplicationSettings.Random, ApplicationSettings.AgentSettings);

                agent.InsertNewBrainAsBits(networkAsBits);
            }
            else
            {
                agent = new Agent(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height),
                                                numberOfEyes, 5, agentType, _agentsColor[(int)agentType], agentType == SpecieType.Herbivore ? typeOneEats : typeTwoEats, ApplicationSettings.Random, ApplicationSettings.AgentSettings);
            }
            agents.Add(agent);
        }


        public void GenerateFood()
        {
            food.Add(new Food(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height), SpecieType.Plant, colorFood, 50));
        }

        public void GenerateBadFood()
        {
            badFood.Add(new Food(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height), SpecieType.Plant, colorBadFood, -50));
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

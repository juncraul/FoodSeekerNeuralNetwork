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

        DateTime timeWhenApplicationStarted;
        DateTime lastTimeFoodWasGenerated;

        int numberOfAgentsTypeOne = 10;
        int numberOfAgentsTypeTwo = 10;
        int numberOfEyes = 16;
        string typeZero = "Berry";
        string typeOne = "One";
        string typeTwo = "Two";
        Color colorZero = Color.GreenYellow;
        Color colorOne = Color.Red;
        Color colorTwo = Color.Blue;
        List<string> typeOneEats = new List<string>();
        List<string> typeTwoEats = new List<string>();

        private ApplicationEngine(Size worldCanvasSize, Size brainCanvasSize)
        {
            timeWhenApplicationStarted = DateTime.Now;
            _geneticEvolution = new GeneticEvolution(ApplicationSettings.Random, 0.007f, 0.7f);
            agents = new List<Agent>();
            food = new List<Food>();
            bitmapWorld = new Bitmap(worldCanvasSize.Width, worldCanvasSize.Height);
            graphicsWorld = Graphics.FromImage(bitmapWorld);

            bitmapBrain = new Bitmap(brainCanvasSize.Width, brainCanvasSize.Height);
            graphicsBrain = Graphics.FromImage(bitmapBrain);

            
            typeOneEats.Add(typeZero);
            typeTwoEats.Add(typeOne);

            for (int i = 0; i < numberOfAgentsTypeOne; i++)
            {
                agents.Add(new Agent(new Vector2(ApplicationSettings.Random.Next() % bitmapWorld.Width, ApplicationSettings.Random.Next() % bitmapWorld.Height), numberOfEyes, typeOne, colorOne, typeOneEats));
            }

            for (int i = 0; i < numberOfAgentsTypeTwo; i++)
            {
                agents.Add(new Agent(new Vector2(ApplicationSettings.Random.Next() % bitmapWorld.Width, ApplicationSettings.Random.Next() % bitmapWorld.Height), numberOfEyes, typeTwo, colorTwo, typeTwoEats));
            }

            for (int i = 0; i < 10; i++)
            {
                GenerateFood();
            }
        }

        public static ApplicationEngine GetInstance(Size worldCanvasSize, Size brainCanvasSize)
        {
            return _applicationEngineInstance = (_applicationEngineInstance == null ? new ApplicationEngine(worldCanvasSize, brainCanvasSize) : _applicationEngineInstance);
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

        public void DoLogic()
        {
            List<BasePopulation> allPopulation = agents.Cast<BasePopulation>().Union(food.Cast<BasePopulation>()).ToList();

            for(int i = agents.Count -1; i >= 0; i --)
            {
                agents[i].CheckEyes(allPopulation);
                agents[i].SendSignalsToBrain();
                agents[i].AgentActivity();
                agents[i].CheckForFood(food, agents);
            }

            agents.RemoveAll(a => a.Energy <= 0);

            for(int i = agents.Count; i < numberOfAgentsTypeOne + numberOfAgentsTypeTwo; i ++)
            {
                if(agents.Count(a=>a.SpecieType == typeOne) != numberOfAgentsTypeOne)
                {
                    string networkAsBits = _geneticEvolution.Reproduce(agents.Where(a => a.SpecieType == typeOne).OrderByDescending(a => a.Energy).ToList()[0].GetBrainAsBits(), 
                                                                       agents.Where(a => a.SpecieType == typeOne).OrderByDescending(a => a.Energy).ToList()[1].GetBrainAsBits());
                    Agent agent = new Agent(new Vector2(ApplicationSettings.Random.Next() % bitmapWorld.Width, ApplicationSettings.Random.Next() % bitmapWorld.Height), numberOfEyes, typeOne, colorOne, typeOneEats);
                    agent.InsertNewBrainAsBits(networkAsBits);
                    agents.Add(agent);
                }
                else
                {
                    string networkAsBits = _geneticEvolution.Reproduce(agents.Where(a => a.SpecieType == typeTwo).OrderByDescending(a => a.Energy).ToList()[0].GetBrainAsBits(),
                                                                       agents.Where(a => a.SpecieType == typeTwo).OrderByDescending(a => a.Energy).ToList()[1].GetBrainAsBits());
                    Agent agent = new Agent(new Vector2(ApplicationSettings.Random.Next() % bitmapWorld.Width, ApplicationSettings.Random.Next() % bitmapWorld.Height), numberOfEyes, typeTwo, colorTwo, typeTwoEats);
                    agent.InsertNewBrainAsBits(networkAsBits);
                    agents.Add(agent);
                }
            }

            TimeSpan timeBetweenFoodGeneration = (DateTime.Now - lastTimeFoodWasGenerated);
            if (timeBetweenFoodGeneration.TotalMilliseconds > ApplicationSettings.GenerateFoodMilliseconds)
            {
                GenerateFood();
            }
        }

        public void GenerateFood()
        {
            lastTimeFoodWasGenerated = DateTime.Now;
            food.Add(new Food(new Vector2(ApplicationSettings.Random.Next() % bitmapWorld.Width, ApplicationSettings.Random.Next() % bitmapWorld.Height), typeZero, colorZero));
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

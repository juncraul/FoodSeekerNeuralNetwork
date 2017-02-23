using Mathematics;
using Population;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FoodSeekerNeuralNetwork
{
    public class ApplicationEngine
    {
        private static ApplicationEngine _applicationEngineInstance;
        public List<Agent> agents;
        public List<Food> food;
        Random random;
        Bitmap bitmapWorld;
        Graphics graphicsWorld;

        Bitmap bitmapBrain;
        Graphics graphicsBrain;

        DateTime timeWhenApplicationStarted;
        DateTime lastTimeFoodWasGenerated;

        private ApplicationEngine(Size worldCanvasSize, Size brainCanvasSize)
        {
            timeWhenApplicationStarted = DateTime.Now;
            agents = new List<Agent>();
            food = new List<Food>();
            random = new Random();
            bitmapWorld = new Bitmap(worldCanvasSize.Width, worldCanvasSize.Height);
            graphicsWorld = Graphics.FromImage(bitmapWorld);

            bitmapBrain = new Bitmap(brainCanvasSize.Width, brainCanvasSize.Height);
            graphicsBrain = Graphics.FromImage(bitmapBrain);

            for (int i = 0; i < 10; i++)
            {
                agents.Add(new Agent(new Vector2(random.Next() % bitmapWorld.Width, random.Next() % bitmapWorld.Height), 10));
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
            foreach (Agent a in agents)
            {
                a.DrawAgent(graphicsWorld, bitmapWorld);
            }
            foreach (Food f in food)
            {
                f.Draw(graphicsWorld, bitmapWorld);
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

            foreach (Agent a in agents)
            {
                a.CheckEyes(allPopulation);
                a.SendSignalsToBrain();
                a.AgentActivity();
                a.CheckForFood(food);
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
            food.Add(new Food(new Vector2(random.Next() % bitmapWorld.Width, random.Next() % bitmapWorld.Height)));
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

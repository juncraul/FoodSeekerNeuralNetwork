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
        Bitmap bitmap;
        Graphics graphics;

        private ApplicationEngine(Size canvasSize)
        {
            agents = new List<Agent>();
            food = new List<Food>();
            random = new Random();
            bitmap = new Bitmap(canvasSize.Width, canvasSize.Height);
            graphics = Graphics.FromImage(bitmap);

            for (int i = 0; i < 10; i++)
            {
                agents.Add(new Agent(new Vector2(random.Next() % bitmap.Width, random.Next() % bitmap.Height), 10));
            }

            for (int i = 0; i < 10; i++)
            {
                food.Add(new Food(new Vector2(random.Next() % bitmap.Width, random.Next() % bitmap.Height)));
            }
        }

        public static ApplicationEngine GetInstance(Size canvasSize)
        {
            return _applicationEngineInstance = (_applicationEngineInstance == null ? new ApplicationEngine(canvasSize) : _applicationEngineInstance);
        }

        public Bitmap Draw()
        {
            SolidBrush brush = new SolidBrush(Color.White);
            graphics.FillRectangle(brush, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
            foreach (Agent a in agents)
            {
                a.Draw(graphics, bitmap);
            }
            foreach (Food f in food)
            {
                f.Draw(graphics, bitmap);
            }

            return bitmap;
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
        }
    }
}

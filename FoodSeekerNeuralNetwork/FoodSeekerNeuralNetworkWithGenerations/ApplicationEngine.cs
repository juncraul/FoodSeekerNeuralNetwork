using GeneticProgramming;
using Mathematics;
using Population;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodSeekerNeuralNetworkWithGenerations
{
    public class ApplicationEngine
    {
        private static ApplicationEngine _applicationEngineInstance;
        private static GeneticEvolution _geneticEvolution;
        public List<Agent> oldAgents;
        public List<Agent> agents;
        public List<Food> food;
        Bitmap bitmapWorld;
        Graphics graphicsWorld;

        Bitmap bitmapBrain;
        Graphics graphicsBrain;

        Bitmap bitmapGraph;
        Graphics graphicsGraph;

        Graph evolutionGraph;


        int numberOfEyes = 16;
        List<string> types = new List<string>();
        Color colorZero = Color.GreenYellow;
        List<string> typeOneEats = new List<string>();
        List<string> typeTwoEats = new List<string>();
        List<Color> _agentsColor;
        int epochNumber;
        int framesInThisEpoch;

        private ApplicationEngine(Size worldCanvasSize, Size brainCanvasSize, Size graphCanvasSize)
        {
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

            for (int i = 0; i < ApplicationSettings.NumberOfAgentsTypeOne; i++)
            {
                GenerateAgent(1, false);
            }

            for (int i = 0; i < ApplicationSettings.NumberOfAgentsTypeTwo; i++)
            {
                GenerateAgent(2, false);
            }

            for (int i = 0; i < ApplicationSettings.FoodOnScreen; i++)
            {
                GenerateFood();
            }

            evolutionGraph = new Graph(200, 500, 2, _agentsColor);
            evolutionGraph.AddPoint(0, 0);
            evolutionGraph.AddPoint(1, 0);
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
            Agent agent = agents.Where(a => a.IsSelected).FirstOrDefault();
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
            framesInThisEpoch++;
            if(framesInThisEpoch == ApplicationSettings.EpochTime)
            {
                StartNewEpoch();
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
                food.RemoveAll(f => !f.IsAlive);
            }
            
            while(food.Count < ApplicationSettings.FoodOnScreen)
            {
                GenerateFood();
            }
        }

        private void StartNewEpoch()
        {
            epochNumber++;
            framesInThisEpoch = 0;
            int sumFoodAte = agents.Where(a => a.SpecieType == types[1]).Sum(a => a.FoodAte);
            evolutionGraph.AddPoint(0, ((float)sumFoodAte / ApplicationSettings.NumberOfAgentsTypeOne));
            evolutionGraph.AddPoint(1, agents.Max(a=>a.FoodAte));

            oldAgents = agents;
            agents = new List<Agent>();
            food = new List<Food>();

            //Keep Elite agents in the new epoch
            for(int i = 0; i < ApplicationSettings.KeepEliteAgents * ApplicationSettings.NumberOfAgentsTypeOne; i ++)
            {
                Agent eliteAgent = oldAgents.OrderByDescending(a => a.FoodAte).ToList()[i];
                DuplicateAgent(1, eliteAgent);
            }

            for (int i = 0; i < ApplicationSettings.NumberOfAgentsTypeOne; i++)
            {
                GenerateAgent(1, true);
            }

            for (int i = 0; i < ApplicationSettings.NumberOfAgentsTypeTwo; i++)
            {
                GenerateAgent(2, true);
            }

            for (int i = 0; i < ApplicationSettings.FoodOnScreen; i++)
            {
                GenerateFood();
            }
        }

        public void GenerateAgent(int agentId, bool isCrossoverAgent)
        {
            Agent agent = new Agent(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height),
                                                numberOfEyes, types[agentId], _agentsColor[agentId - 1], agentId == 1 ? typeOneEats : typeTwoEats, ApplicationSettings.Random, ApplicationSettings.AgentSettings);
            if (isCrossoverAgent)
            {
                if (agents.Count(a => a.SpecieType == types[agentId]) <= 1)
                {
                    GenerateAgent(agentId, false);
                    return;
                }
                int indexParent0 = _geneticEvolution.Roulette(oldAgents.Where(a => a.SpecieType == types[agentId])
                                                                    .Select(a => a.FoodAte * ApplicationSettings.ScoreForEatingFood).ToArray());
                int indexParent1 = _geneticEvolution.Roulette(oldAgents.Where(a => a.SpecieType == types[agentId])
                                                                    .Select(a => a.FoodAte * ApplicationSettings.ScoreForEatingFood).ToArray());
                string networkAsBits = _geneticEvolution.Reproduce(oldAgents[indexParent0].GetBrainAsBits(),
                                                            oldAgents[indexParent1].GetBrainAsBits());
                agent.InsertNewBrainAsBits(networkAsBits);
            }
            agents.Add(agent);
        }

        public void DuplicateAgent(int agentId, Agent from)
        {
            Agent agent = new Agent(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height),
                                                numberOfEyes, types[agentId], _agentsColor[agentId - 1], agentId == 1 ? typeOneEats : typeTwoEats, ApplicationSettings.Random, ApplicationSettings.AgentSettings);
            string agentBrain = from.GetBrainAsBits();
            agent.InsertNewBrainAsBits(agentBrain);
            agents.Add(agent);
        }

        public void GenerateFood()
        {
            if (food.Count > agents.Count * 2.5)
                return;
            food.Add(new Food(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height), types[0], colorZero));
        }

        public void SelectAgent(Point p)
        {
            foreach (Agent a in agents)
            {
                a.IsSelected = false;
            }

            foreach (Agent a in agents)
            {
                if (Functions.CollisionPointCircle(new Vector2(p.X, p.Y), a.Position, a.Radius))
                {
                    a.IsSelected = true;
                    break;
                }
            }
        }

        public string GiveInfo()
        {
            string text = string.Empty;
            text += "Epoch number: " + epochNumber + Environment.NewLine;
            text += "Frames in epoch: " + framesInThisEpoch + Environment.NewLine;
            return text;
        }
    }
}

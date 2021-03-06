﻿using GeneticProgramming;
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
        public List<Food> badFood;
        Bitmap bitmapWorld;
        Graphics graphicsWorld;

        Bitmap bitmapBrain;
        Graphics graphicsBrain;

        Bitmap bitmapGraph;
        Graphics graphicsGraph;

        Graph evolutionGraph;

        
        Color colorFood = Color.GreenYellow;
        Color colorBadFood = Color.Gray;
        List<SpecieType> typeOneEats = new List<SpecieType>();
        List<SpecieType> typeTwoEats = new List<SpecieType>();
        List<Color> _agentsColor;
        int epochNumber;
        int framesInThisEpoch;

        private ApplicationEngine(Size worldCanvasSize, Size brainCanvasSize, Size graphCanvasSize)
        {
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
            _agentsColor.Add(Color.Green);
            _agentsColor.Add(Color.Red);
            _agentsColor.Add(Color.Blue);

            for (int i = 0; i < ApplicationSettings.NumberOfAgentsTypeOne; i++)
            {
                Agent a = GenerateAgent(SpecieType.Herbivore, false, null);
                if (a != null)
                    agents.Add(a);
            }

            for (int i = 0; i < ApplicationSettings.NumberOfAgentsTypeTwo; i++)
            {
                Agent a = GenerateAgent(SpecieType.Carnivore, false, null);
                if (a != null)
                    agents.Add(a);
            }

            for (int i = 0; i < ApplicationSettings.FoodOnScreen; i++)
            {
                food.Add(CreateNewFood(colorFood, 50));
            }

            for (int i = 0; i < ApplicationSettings.BadFoodOnScreen; i++)
            {
                badFood.Add(CreateNewFood(colorBadFood, -50));
            }

            evolutionGraph = new Graph(200, 500, 3, _agentsColor);
            evolutionGraph.AddPoint(0, 0);
            evolutionGraph.AddPoint(1, 0);
            evolutionGraph.AddPoint(2, 0);
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
            foreach (Agent a in agents.Where(a=>a.IsAlive))
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
            if(framesInThisEpoch == ApplicationSettings.EpochTime || agents.Count(a=>a.IsAlive) == 0)
            {
                StartNewEpoch();
            }

            List<BasePopulation> allPopulation = agents.Cast<BasePopulation>().Union(food.Cast<BasePopulation>()).Union(badFood.Cast<BasePopulation>()).ToList();

            for (int i = agents.Count - 1; i >= 0; i--)
            {
                if (!agents[i].IsAlive)
                    continue;
                agents[i].AgentActivity();
                agents[i].CheckEyes(allPopulation);
                agents[i].TrainTheAgent();
                agents[i].CheckForFood(food.Union(badFood.Cast<Food>()).ToList(), agents);
                food.RemoveAll(f => !f.IsAlive);
                badFood.RemoveAll(f => !f.IsAlive);
            }
            
            while(food.Count < ApplicationSettings.FoodOnScreen)
            {
                food.Add(CreateNewFood(colorFood, 50));
            }

            while (badFood.Count < ApplicationSettings.BadFoodOnScreen)
            {
                badFood.Add(CreateNewFood(colorBadFood, -50));
            }
        }

        private void StartNewEpoch()
        {
            epochNumber++;
            framesInThisEpoch = 0;
            int sumFoodAte = agents.Where(a => a.Type == SpecieType.Herbivore).Sum(a => a.GoodFoodAte);
            evolutionGraph.AddPoint(0, ((float)sumFoodAte / ApplicationSettings.NumberOfAgentsTypeOne));
            evolutionGraph.AddPoint(1, agents.Max(a=>a.GoodFoodAte));
            evolutionGraph.AddPoint(2, agents.Average(a=>a.GetHiddenNeurons()));

            oldAgents = agents;
            agents = new List<Agent>();
            food = new List<Food>();
            badFood = new List<Food>();

            //Keep Elite agents in the new epoch
            for(int i = 0; i < (int)(ApplicationSettings.KeepEliteAgents * ApplicationSettings.NumberOfAgentsTypeOne); i ++)
            {
                Agent eliteAgent = oldAgents.OrderByDescending(a => a.GetFitness(ApplicationSettings.ScoreForEatingGoodFood, ApplicationSettings.ScoreForEatingBadFood, ApplicationSettings.ScoreForExisting)).ToList()[i];
                DuplicateAgent(SpecieType.Herbivore, eliteAgent);
            }

            for (int i = agents.Count(a => a.Type == SpecieType.Herbivore); i < ApplicationSettings.NumberOfAgentsTypeOne; i++)
            {
                Agent a = GenerateAgent(SpecieType.Herbivore, true, oldAgents);
                if (a != null)
                    agents.Add(a);
            }

            for (int i = agents.Count(a => a.Type == SpecieType.Carnivore); i < ApplicationSettings.NumberOfAgentsTypeTwo; i++)
            {
                Agent a = GenerateAgent(SpecieType.Carnivore, true, oldAgents);
                if (a != null)
                    agents.Add(a);
            }

            for (int i = 0; i < ApplicationSettings.FoodOnScreen; i++)
            {
                food.Add(CreateNewFood(colorFood, 50));
            }

            for (int i = 0; i < ApplicationSettings.BadFoodOnScreen; i++)
            {
                badFood.Add(CreateNewFood(colorBadFood, -50));
            }
        }

        public Agent GenerateAgent(SpecieType agentType, bool isCrossoverAgent, List<Agent> agents)
        {
            Agent agent;

            if (isCrossoverAgent)
            {
                if (agents.Count(a => a.Type == agentType) <= 1)
                {
                    GenerateAgent(agentType, false, agents);
                    return null;
                }

                Agent parent0 = GetRandomRoulleteAgentFromList(agents, agentType);
                Agent parent1 = GetRandomRoulleteAgentFromList(agents.Except(new List<Agent> { parent0 }).ToList(), agentType);
                
                string brainOfParent0 = parent0.GetBrainAsBits();
                string brainOfParent1 = parent1.GetBrainAsBits();

                string smaller = brainOfParent0.Length < brainOfParent1.Length ? brainOfParent0 : brainOfParent1;
                string bigger = brainOfParent0.Length > brainOfParent1.Length ? brainOfParent0 : brainOfParent1;

                int hiddenNeurons;

                if (ApplicationSettings.Random.Next() % 2 == 0)
                {//append it
                    smaller = smaller + bigger.Substring(smaller.Length);
                    hiddenNeurons = brainOfParent0.Length > brainOfParent1.Length ? parent0.GetHiddenNeurons() : parent1.GetHiddenNeurons();
                }
                else
                {//cut it
                    bigger = bigger.Substring(0, smaller.Length);
                    hiddenNeurons = brainOfParent0.Length < brainOfParent1.Length ? parent0.GetHiddenNeurons() : parent1.GetHiddenNeurons();
                }
                brainOfParent0 = smaller;
                brainOfParent1 = bigger;

                string networkAsBits = _geneticEvolution.Reproduce(brainOfParent0, brainOfParent1);
                
                //hiddenNeurons += ApplicationSettings.Random.Next() % 100 < 10 ? 1 : 0;//Chance to mutate an extra hidden neuron
                //hiddenNeurons += ApplicationSettings.Random.Next() % 100 < 10 ? -1 : 0;//Chance to mutate and lose a hidden neuron

                agent = CreateNewAgent(hiddenNeurons, agentType);

                agent.InsertNewBrainAsBits(networkAsBits);
            }
            else
            {
                agent = CreateNewAgent(ApplicationSettings.HiddenNeurons, agentType);
            }
            return agent;
        }

        public Agent GetRandomRoulleteAgentFromList(List<Agent> agents, SpecieType agentType)
        {
            int index = _geneticEvolution.Roulette(agents.Where(a => a.Type == agentType)
                                                                .Select(a => a.GetFitness(ApplicationSettings.ScoreForEatingGoodFood, ApplicationSettings.ScoreForEatingBadFood, ApplicationSettings.ScoreForExisting)).ToArray());
            return agents[index];
        }

        public void DuplicateAgent(SpecieType agentType, Agent fromAgent)
        {
            int hiddenNeurons = fromAgent.GetHiddenNeurons();
            Agent agent = CreateNewAgent(hiddenNeurons, agentType);
            string agentBrain = fromAgent.GetBrainAsBits();
            agent.InsertNewBrainAsBits(agentBrain);
            agents.Add(agent);
        }

        Agent CreateNewAgent(int hiddenNeurons, SpecieType agentType)
        {
            return new Agent(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height),
                                            ApplicationSettings.NumberOfEyes, hiddenNeurons, agentType, _agentsColor[(int)agentType], agentType == SpecieType.Herbivore ? typeOneEats : typeTwoEats, ApplicationSettings.Random, ApplicationSettings.AgentSettings);
        }

        public Food CreateNewFood(Color foodColor, int foodValue)
        {
            return new Food(new Vector2(ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Width, ApplicationSettings.Random.Next() % ApplicationSettings.SpawningSpace.Height), SpecieType.Plant, foodColor, foodValue);
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

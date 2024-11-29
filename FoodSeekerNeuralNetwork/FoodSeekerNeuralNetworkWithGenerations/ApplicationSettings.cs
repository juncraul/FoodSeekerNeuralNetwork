using Population;
using System;
using System.Drawing;

namespace FoodSeekerNeuralNetworkWithGenerations
{
    public static class ApplicationSettings
    {
        //seed is set to 0 for debug purpose
        public static Random Random = new Random(0);
        public static float MutationRate = 0.2f;
        public static float CrossOverRate = 0.7f;
        public static double ScoreForEatingGoodFood = 50;
        public static double ScoreForEatingBadFood = -150;
        public static double ScoreForExisting = 10;
        public static Size SpawningSpace = new Size(724, 581); // new Size(800, 800);
        public static int NumberOfAgentsTypeOne = 15;
        public static int NumberOfAgentsTypeTwo = 0;
        public static AgentSettings AgentSettings = AgentSettings.ShowFoodAte | AgentSettings.ShowEnergy;
        public static int EpochTime = 1000;
        public static int FoodOnScreen = 30;
        public static int BadFoodOnScreen = 5;
        public static float KeepEliteAgents = 0.2f;
        public static int NumberOfEyes = 13;
        public static int HiddenNeurons = 15;
    }
}
using System;
using System.Drawing;

namespace FoodSeekerNeuralNetwork
{
    public static class ApplicationSettings
    {
        //seed is set to 0 for debug purpose
        public static Random Random = new Random(0);
        public static float MutationRate = 0.07f;
        public static float CrossOverRate = 0.7f;
        public static float ScoreForEatingFood = 50;
        public static Size SpawningSpace = new Size(724, 581);
        public static int NumberOfAgentsTypeOne = (int)((SpawningSpace.Width + SpawningSpace.Height) / 100);
        public static int NumberOfAgentsTypeTwo = 0;
        public static int GenerateFoodMilliseconds = 500;
        public static int FoodOnScreen = 30;
    }
}

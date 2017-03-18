using System;

namespace FoodSeekerNeuralNetwork
{
    public static class ApplicationSettings
    {
        //seed is set to 0 for debug purpose
        public static Random Random = new Random(0);
        public static int GenerateFoodMilliseconds = 1000;
        public static float MutationRate = 0.007f;
        public static float CrossOverRate = 0.7f;
        public static float ScoreForEatingFood = 50;
    }
}

using System;

namespace FoodSeekerNeuralNetwork
{
    public static class ApplicationSettings
    {
        //seed is set to 0 for debug purpose
        public static Random Random = new Random(0);
        public static int GenerateFoodMilliseconds = 1000;
    }
}

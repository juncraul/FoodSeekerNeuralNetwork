using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Population;

namespace FoodSeekerNeuralNetworkWithGenerations
{
    public static class ApplicationSettings
    {
        //seed is set to 0 for debug purpose
        public static Random Random = new Random(0);
        public static float MutationRate = 0.07f;
        public static float CrossOverRate = 0.7f;
        public static double ScoreForEatingFood = 50;
        public static Size SpawningSpace = new Size(724, 581);// new Size(800, 800);
        public static int NumberOfAgentsTypeOne = (int)((SpawningSpace.Width + SpawningSpace.Height) / 100);
        public static int NumberOfAgentsTypeTwo = 0;
        public static AgentSettings AgentSettings = AgentSettings.DontDecay | AgentSettings.ShowFootAte;
        public static int EpochTime = 1000;
        public static int FoodOnScreen = 30;
        public static float KeepEliteAgents = 0.2f;
    }
}

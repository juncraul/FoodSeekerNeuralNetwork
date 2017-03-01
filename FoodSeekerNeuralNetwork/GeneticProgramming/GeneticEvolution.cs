using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticProgramming
{
    public class GeneticEvolution
    {
        private Random _random;
        private readonly float _mutation_rate = 0.007f;

        public GeneticEvolution(Random random, float mutation_rate, float crossover_rate)
        {
            _random = random;
            _mutation_rate = mutation_rate;
            //_crossover_rate = crossover_rate;
        }

        public string Reproduce(string parent0, string parent1)
        {
            string offspring = Crossover(parent0, parent1);
            return Mutate(offspring);
        }

        public string Mutate(string bits)
        {
            string output = "";

            for (int i = 0; i < bits.Length; i++)
            {
                if (_random.NextDouble() < _mutation_rate)
                {
                    if (bits[i] == '1')

                        output += "0";

                    else

                        output += "1";
                }
                else
                {
                    output += bits[i];
                }
            }

            return output;
        }

        public string Crossover(string parent0, string parent1)
        {
            int crossoverPivot = (int)(_random.NextDouble() * parent0.Length);

            return parent0.Substring(0, crossoverPivot) + parent1.Substring(crossoverPivot);
        }

        //public string Roulette(float total_fitness, chromo_typ[] Population)
        //{
        //    //generate a random number between 0 & total fitness count
        //    float Slice = (float)(_random.NextDouble() * total_fitness);

        //    //go through the chromosones adding up the fitness so far
        //    float FitnessSoFar = 0.0f;

        //    for (int i = 0; i < Population.Length; i++)
        //    {
        //        FitnessSoFar += Population[i].fitness;

        //        //if the fitness so far > random number return the chromo at this point
        //        if (FitnessSoFar >= Slice)

        //            return Population[i].bits;
        //    }

        //    return "";
        //}
    }
}

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

        string Mutate(string bits)
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

        string Crossover(string parent0, string parent1)
        {
            int crossoverPivot = (int)(_random.NextDouble() * parent0.Length);

            return parent0.Substring(0, crossoverPivot) + parent1.Substring(crossoverPivot);
        }

        public int Roulette(double[] fitness)
        {
            double totalFitness = fitness.Sum(a=>a);

            //generate a random number between 0 & total fitness count
            double slice = (float)(_random.NextDouble() * totalFitness);

            //go through the chromosones adding up the fitness so far
            double fitnessSoFar = 0.0f;

            for (int i = 0; i < fitness.Length; i++)
            {
                fitnessSoFar += fitness[i];

                //if the fitness so far > random number return the chromo at this point
                if (fitnessSoFar >= slice)
                    return i;
            }
            return -1;
        }
    }
}

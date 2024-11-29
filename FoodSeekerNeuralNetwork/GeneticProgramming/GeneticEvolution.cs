using System;
using System.Linq;

namespace GeneticProgramming
{
    public class GeneticEvolution
    {
        private readonly Random _random;
        private readonly float _mutationRate;// = 0.007f;

        public GeneticEvolution(Random random, float mutationRate, float crossoverRate)
        {
            _random = random;
            _mutationRate = mutationRate;
            //_crossover_rate = crossover_rate;
        }

        public string Reproduce(string parent0, string parent1)
        {
            var offspring = Crossover(parent0, parent1, true);
            return Mutate(offspring);
        }

        private string Mutate(string bits)
        {
            var output = "";

            foreach (var bit in bits)
            {
                if (_random.NextDouble() < _mutationRate)
                {
                    if (bit == '1')

                        output += "0";

                    else

                        output += "1";
                }
                else
                {
                    output += bit;
                }
            }

            return output;
        }

        private string Crossover(string parent0, string parent1, bool allowDifferentLength)
        {
            if (parent0.Length != parent1.Length && allowDifferentLength)
            {
                if (allowDifferentLength)
                    CrossoverDifferentSize(ref parent0, ref parent1);
                else
                    throw new Exception("Different length crossover is not allowed");
            }

            var crossoverPivot = (int) (_random.NextDouble() * parent0.Length);

            return parent0.Substring(0, crossoverPivot) + parent1.Substring(crossoverPivot);
        }

        private void CrossoverDifferentSize(ref string parent0, ref string parent1)
        {
            var smaller = parent0.Length > parent1.Length ? parent0 : parent1;
            var bigger = parent0.Length < parent1.Length ? parent0 : parent1;

            if (_random.Next() % 2 == 0)
            {
                //append it
                smaller = smaller + bigger.Substring(smaller.Length);
            }
            else
            {
                //cut it
                bigger = bigger.Substring(0, smaller.Length);
            }

            parent0 = smaller;
            parent1 = bigger;
        }

        public int Roulette(double[] fitness)
        {
            var totalFitness = fitness.Sum(a => a);

            //generate a random number between 0 & total fitness count
            double slice = (float) (_random.NextDouble() * totalFitness);

            //go through the chromosones adding up the fitness so far
            double fitnessSoFar = 0.0f;

            for (var i = 0; i < fitness.Length; i++)
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
using System;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //TestRandomCase();

            int length = 10;
            var random = new Random();
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(Math.Floor(2 * random.NextDouble()).ToString());
            }

            var goal = sb.ToString();
            Func<string, double> f = Fitness(goal);

            NextGeneration();

            Console.WriteLine(goal);
            Console.WriteLine(GeneticAlgorithm.FindBinaryGeneticString(f, length, 0.6, 0.002));
            Console.ReadKey();
        }

        public static void NextGeneration()
        {
            // Arrange
            int solutiondSize = 4;
            int geneCount = 6;
            int crossOverChance = 30;
            int mutationChance = 5;
            Solution _solution = new Solution(geneCount, solutiondSize, crossOverChance, mutationChance);
            _solution.InitializePopulation();

            // Act
            int generation = 0;
            Genome _idealSum = null;
            while (_idealSum == null)
            {
                _solution.Mutate();
                _solution.CrossOver();
                _solution.NextGeneration();
                Console.WriteLine(_solution);

                generation++;
                _idealSum = _solution.IdealSum();
            }

            Console.WriteLine("A new idealSum! generation {0} - {1}", generation, _idealSum);
        }

        private static Func<string, double> Fitness(string goal)
        {
            return new Func<string, double>(chromosome => {
                double total = 0;

                for (int i = 0; i < goal.Length; i++)
                {
                    if (goal[i] != chromosome[i])
                    {
                        total++;
                    }
                }

                return 1.0 / (total + 1);
            });
        }

    }
}

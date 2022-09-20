using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    // step 1: to generate a population of random chromosomes.

    internal class GeneticAlgorithm
    {

        /// <summary>
		/// This World's population of genomes.
		/// </summary>
        public IList<Genome> Population { get; set; }


        /// <summary>
		/// The number of genomes in the world - should be an even number.
		/// </summary>
        private int _populationSize;

        /// <summary>
		/// The number of bits in the gene.
		/// </summary>
        private int _geneSize;


        /// <summary>
        /// One internal parameter available to you is the size of your population. A number between 50 and 100 chromosomes is reasonable.
        /// </summary>
        /// <param name="fitness">Provided function that computes the fitness of a chromosome</param>
        /// <param name="length">A number of the bits in the target chromosome.</param>
        /// <param name="probCrossover">Provided floating point numbers that represent the chance of their respective modification occuring</param>
        /// <param name="probMutation">Provided floating point numbers that represent the chance of their respective modification occuring</param>
        /// <returns></returns>
        public static string FindBinaryGeneticString(Func<string, double> fitness,
                                             int length, double probCrossover,
                                             double probMutation)
        {


            //generate a population of random chromosomes.
            Solution solution = new Solution(6, 50, 2, 1);
            solution.InitializePopulation();

            return "1";
        }

        public void InitializePopulation()
        {
            Population = new List<Genome>();

            for (int i = 0; i < _populationSize; i++)
            {
                Genome genome = new Genome(_geneSize);
                genome.RandomizeGeneValues();

                Population.Add(genome);
            }
        }
    }

    public class Genome
    {
        private bool[] _genes;
        public Guid Id { get; set; }

        private static Random _random = new Random();

        public Genome(int maxGenes)
        {
            _genes = new bool[maxGenes];
            Id = Guid.NewGuid();
        }

        public IEnumerable<bool> Genes
        {
            get { return _genes; }
        }

        public void RandomizeGeneValues()
        {
            for (int i = 0; i < _genes.Length; i++)
            {
                // The gene is "1" when the rng is over 50
                if (_random.Next(1, 100) > 50)
                {
                    _genes[i] = true;
                }
                else
                {
                    _genes[i] = false;
                }
            }
        }

        public int Total
        {
            get
            {
                // TODO CALCULATE VALUE
                return _random.Next(1, 100);

            }
        }

        public void SwapGenes(int position1, int position2)
        {
            bool position1Value = _genes[position1];
            bool position2Value = _genes[position2];
            _genes[position1] = position2Value;
            _genes[position2] = position1Value;
        }

        public void SwapWith(Genome genome, int toPosition)
        {
            List<bool> sourceGenes = genome.Genes.ToList();
            for (int i = 0; i < toPosition; i++)
            {
                _genes[i] = sourceGenes[i];
            }
        }

        public Genome Clone()
        {
            Genome clonedGenome = new Genome(_genes.Length);
            List<bool> genes = Genes.ToList();
            for (int i = 0; i < _genes.Length; i++)
            {
                clonedGenome._genes[i] = genes[i];
            }

            return clonedGenome;
        }
    }

    public class Solution
    {
        private static Random _random = new Random();

        /// <summary>
		/// The number of bits in the gene.
		/// </summary>
        private int _geneSize;

        /// <summary>
        /// The number of genomes in the world - should be an even number.
        /// </summary>
        private int _populationSize;

        /// <summary>
        /// The percentage chance of a cross over (genes being swapped in two genomes) each generation.
        /// </summary>
        private int _crossOverChance;

        /// <summary>
        /// The percentage chance of a mutation (a bit being flipped in a random position) over each generation.
        /// </summary>
        private int _mutationChance;

        /// <summary>
		/// Display console logging when mutations and cross overs don't occur.
		/// </summary>
	    public bool ShowDebugMessages { get; set; }


        public IList<Genome> Population { get; set; }

        public Solution(int geneSize, int populationSize, int crossOverChance, int mutationChance)
        {
            _geneSize = geneSize;
            _populationSize = populationSize;
            _crossOverChance = crossOverChance;
            _mutationChance = mutationChance;
            Population = new List<Genome>();
            InitializePopulation();
        }

        public void InitializePopulation()
        {
            Population = new List<Genome>();

            for (int i = 0; i < _populationSize; i++)
            {
                Genome genome = new Genome(_geneSize);
                genome.RandomizeGeneValues();

                Population.Add(genome);
            }
        }

        //Calculate the fitness of those chromosomes
        public virtual int FitnessFunction(Genome genome)
        {
            return genome.Total;
        }


        public void Mutate(Random random = null)
        {
            EnsurePopulationIsCreated();

            if (random == null)
                random = _random;

            // Figure out if mutation should occur for this generation, based on a roll of a random number
            decimal percentage = _mutationChance;
            int randomNumber = random.Next(1, 100);

            if (percentage > 0 && randomNumber <= percentage)
            {
                // Loop through all genome pairs
                for (int i = 0; i < Population.Count; i += 2)
                {
                    if (i > Population.Count)
                        break;

                    Genome genome = Population[i];

                    // Pick two random positions to swap at
                    int position1 = random.Next(0, _geneSize);
                    int position2 = random.Next(0, _geneSize);
                    genome.SwapGenes(position1, position2);
                }
            }
            else
            {
                // (No mutation, return)
                if (ShowDebugMessages)
                    Console.WriteLine("No mutation performed - the random {0}% was over the {1}% threshold.", randomNumber, percentage);
            }
        }

        public void CrossOver(Random random = null)
        {
            EnsurePopulationIsCreated();

            if (random == null)
                random = _random;

            // Figure out if crossover should occur for this generation, based on a roll of a random number
            decimal percentage = _crossOverChance;
            int randomNumber = random.Next(1, 100);

            if (percentage > 0 && randomNumber <= percentage)
            {
                // Loop through all genome pairs
                for (int i = 0; i < Population.Count; i += 2)
                {
                    if (i > Population.Count)
                        break;

                    Genome genome1 = Population[i];
                    Genome genome2 = Population[i + 1];

                    // Pick a random position to swap at
                    int position = random.Next(0, _geneSize);

                    // Create 2 new genomes with the two parts swapped
                    Genome newGenome1 = genome1.Clone();
                    Genome newGenome2 = genome2.Clone();

                    newGenome1.SwapWith(genome2, position);
                    newGenome2.SwapWith(genome1, position);

                    Population[i] = newGenome1;
                    Population[i + 1] = newGenome2;
                }
            }
            else
            {
                // (No cross over, return)
                if (ShowDebugMessages)
                    Console.WriteLine("No crossover performed - the random {0}% was over the {1}% threshold.", randomNumber, percentage);
            }
        }

        public void NextGeneration()
        {
            EnsurePopulationIsCreated();

            List<Genome> nextGeneration = new List<Genome>();

            for (int i = 0; i < Population.Count; i++)
            {
                Genome genome = SpinBiasedRouletteWheel();
                nextGeneration.Add(genome);
            }

            Population = nextGeneration;
        }

        public Genome SpinBiasedRouletteWheel(Random random = null)
        {
            EnsurePopulationIsCreated();

            if (random == null)
                random = _random;

            // Get the total fitness value of all genomes
            int populationTotal = Population.Sum(x => x.Total);

            for (int i = 0; i < Population.Count; i++)
            {
                Genome genome = Population[i];

                // Weighted % value of each genome: genome fitness value/ total
                // This % represents the chance the genome is picked.
                decimal percentage = ((decimal)genome.Total / populationTotal) * 100;

                // Roll 1-100. If the % lies within in this number, return it.
                // For example: 
                //	percentage is 60%
                //	random number is 75
                //  = doesn't get picked
                int randomNumber = random.Next(1, 100);
                if (percentage <= 0 || randomNumber <= percentage)
                {
                    return genome;
                }
            }

            return Population.First();
        }

        public Genome IdealSum()
        {
            for (int i = 0; i < Population.Count; i++)
            {
                if (FitnessFunction(Population[i]) == 38)
                    return Population[i];
            }

            return null;
        }

        private void EnsurePopulationIsCreated()
        {
            if (Population.Count == 0)
                throw new InvalidOperationException("The population is empty! Use InitializePopulation() first");
        }
    }

}

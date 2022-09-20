# genetic-algorithm

Genetic Algorithms Background Genetic algorithms (GA) are a useful tool for machine learning. 
GA is a general algorithm paradigm that offers a way to find or approximate an answer to problems that may be otherwise intractible.  
For an example problem, given the list [1,2,3,4,5,6,7,8,9,10], find a way to partition the list into two lists such that the sum of one list is 38 and the product of the other list is 210. A brute force solution can work by finding the divisors of 210. However, a this approach quickly becomes intractable on large lists.  

Genetic algorithms are based on the idea that you can create a chromosome that represents a potential solution to the problem. One way of representing such a chromosome is with a binary string of digits. In our example problem above, we could represent one chromosome as 0101110000, and decide that 0 means that the corresponding number is in the "sum" pile and 1 means it is in the "product" pile. 
The 0101110000 chromosome gives us 1+3+7+8+9+10=38 and 2*4*5*6=240--a potential candidate solution with characteristics of our desired solution.  A population is a collection of chromosomes. The "genetic" part of the algorithm's name comes from the fact that the algorithm uses evolution-based concepts to improve the fitness (proximity to the target solution) of the chromosomes.  

The first step in the algorithm is to generate a population of random chromosomes. Calculate the fitness of those chromosomes in whatever way fits the problem. In the problem at hand, the goal is to minimize the absolute difference of the sum from the ideal sum and likewise for the product. 
A fitness score for a chromosome can be the "distance" to the target solution:  

sqrt((chromosome sum - ideal sum)^2 +      (chromosome product - ideal product)^2) 

The closer this score is to 0 (that is, the closer the chromosome is to ideal), the better the fitness. Let our fitness be a decimal in the range 0-1 given by the formula 1 / (score + 1). A fitness of 0 is competely incorrect and a fitness of 1 means an exact answer has been found.  The evolutionary step involves iteratively inspecting the population and selecting chromosomes by fitness which should live in the next generation. 

Each iteration proceeds as follows:  Select two chromosomes from the original population. This is not done purely randomly. This is done using roulette wheel selection, where the chances of picking a chromosome are proportional to its fitness. With a probability probCrossover, a crossover occurs between these two new chromosomes. 
A crossover involves selecting a random cutoff index in two equal-length chromosomes and swapping their tails. For example, a crossover at the 3rd bit between 01011010 and 11110110 results in 010 10110 and 111 11010. With a probability probMutation, a mutation can occur at any bit along each new chromosome; the mutation rate is typically very small. 
For the problem at hand, a mutation entails flipping a bit. Add these two (potentially modified) chromosomes into the new population and repeat steps 1-3 until the new population is the same size as the original one. After performing this evoltionary process iteratively, a point will be reached when a chromosome has attained the target fitness. For this challenge, terminate when a chromosome has a fitness of 1 and return this chromosome.  Your task Your task for this challenge is to complete the function  findBinaryString(getFitness, chromosomeLength, probMutation, probCrossover) 
The first parameter, getFitness is a provided function that computes the fitness of a chromosome.  chromosomeLength is a number of the bits in the target chromosome.  probMutation and probCrossover are provided floating point numbers that represent the chance of their respective modification occuring.  
One internal parameter available to you is the size of your population. A number between 50 and 100 chromosomes is reasonable.  

findBinaryGeneticString should return a binary string of '0' and '1' of chromosomeLength that has a fitness score of 1 as computed by getFitness

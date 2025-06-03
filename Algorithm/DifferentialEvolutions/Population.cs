namespace SLAPScheduling.Algorithm.DifferentialEvolutions
{
    public class Population
    {
        public List<Individual> Solutions { get; set; }

        public Population(List<Individual> solutions)
        {
            Solutions = solutions ?? new List<Individual>();
        }

        /// <summary>
        /// Initializes the population with random individuals.
        /// Each individual has a specified number of dimensions with random location indices within the given domain.
        /// </summary>
        /// <param name="dimensions">Number of elements (sub-lots) in each individual</param>
        /// <param name="minValue">Minimum value of the domain (inclusive)</param>
        /// <param name="maxValue">Maximum value of the domain (exclusive)</param>
        public void InitializeRandom(int dimensions, int minValue, int maxValue)
        {
            if (dimensions <= 0)
                throw new ArgumentException("Dimensions must be positive.", nameof(dimensions));
            if (minValue >= maxValue)
                throw new ArgumentException("minValue must be less than maxValue.", nameof(minValue));

            Solutions.Clear(); // Clear existing solutions
            Random rand = RandomGenerator.Instance.Random;

            // Number of individuals to create (can be adjusted based on Parameters.AgentsCount)
            int populationSize = Math.Max(10, dimensions * 2); // Default size, adjust as needed

            for (int i = 0; i < populationSize; i++)
            {
                Individual individual = new Individual();
                List<int> availableIndices = Enumerable.Range(minValue, maxValue - minValue).ToList();
                availableIndices.ShuffleFast();

                // Assign random locations, allowing duplicates if a location can hold multiple sub-lots
                // If uniqueness is required per individual, use the commented section below
                for (int j = 0; j < dimensions; j++)
                {
                    int randomIndex = availableIndices[rand.Next(0, availableIndices.Count)];
                    individual.Elements.Add(randomIndex);
                }

                // Alternative: Ensure unique assignments per individual (uncomment if needed)
                /*
                if (availableIndices.Count >= dimensions)
                {
                    for (int j = 0; j < dimensions; j++)
                    {
                        int randomIndex = availableIndices[j];
                        individual.Elements.Add(randomIndex);
                    }
                }
                else
                {
                    // If not enough unique locations, allow duplicates
                    for (int j = 0; j < dimensions; j++)
                    {
                        int randomIndex = rand.Next(minValue, maxValue);
                        individual.Elements.Add(randomIndex);
                    }
                }
                */

                Solutions.Add(individual);
            }
        }

        /// <summary>
        /// Evaluates the fitness of all individuals in the population.
        /// </summary>
        public void Evaluate(List<ReceiptSublot> receiptSubLots, Dictionary<int, Location> locationDictionary, int numberOfReceiptSubLots)
        {
            foreach (var individual in Solutions)
            {
                individual.Evaluate(receiptSubLots, locationDictionary, numberOfReceiptSubLots);
            }
        }

        /// <summary>
        /// Gets the best individual based on fitness.
        /// </summary>
        public Individual GetBest()
        {
            return Solutions.OrderBy(i => i.Fitness).First();
        }
    }
}

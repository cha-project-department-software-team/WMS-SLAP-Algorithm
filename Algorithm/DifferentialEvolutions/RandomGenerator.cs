namespace SLAPScheduling.Algorithm.DifferentialEvolutions
{
    public class RandomGenerator
    {
        // Singleton instance for RandomGenerator
        private static readonly RandomGenerator instance = new RandomGenerator();
        public static RandomGenerator Instance => instance;

        // Random number generator
        public Random Random { get; private set; }

        private RandomGenerator()
        {
            Random = new Random();
        }

        /// <summary>
        /// Generates a list of unique random integers within a specified range, excluding a specific index.
        /// </summary>
        /// <param name="count">Number of random indices to generate</param>
        /// <param name="minValue">Minimum value (inclusive)</param>
        /// <param name="maxValue">Maximum value (exclusive)</param>
        /// <param name="excludeIndex">Index to exclude from the result</param>
        /// <returns>List of unique random indices</returns>
        /// <exception cref="ArgumentException">Thrown if count is too large or invalid</exception>
        public List<int> GenerateRandom(int count, int minValue, int maxValue, int excludeIndex = -1)
        {
            // Validate input parameters
            if (count < 0)
                throw new ArgumentException("Count must be non-negative.", nameof(count));
            if (minValue >= maxValue)
                throw new ArgumentException("minValue must be less than maxValue.", nameof(minValue));

            // Calculate the total number of possible values, accounting for the excluded index
            int possibleValues = maxValue - minValue;
            if (excludeIndex >= minValue && excludeIndex < maxValue)
                possibleValues--; // Reduce by 1 if excludeIndex is within range

            if (count > possibleValues)
                throw new ArgumentException($"Cannot generate {count} unique numbers in range [{minValue}, {maxValue}) while excluding {excludeIndex}.", nameof(count));

            List<int> result = new List<int>();
            HashSet<int> usedIndices = new HashSet<int>();

            // Generate unique random indices
            while (result.Count < count)
            {
                int randomIndex = Random.Next(minValue, maxValue);

                // Skip if the index is the excluded one or already used
                if (randomIndex == excludeIndex || usedIndices.Contains(randomIndex))
                    continue;

                usedIndices.Add(randomIndex);
                result.Add(randomIndex);
            }

            return result;
        }

        /// <summary>
        /// Generates a double value within a specified range.
        /// </summary>
        public double GetDoubleRangeRandomNumber(double min, double max)
        {
            return Random.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Shuffles a list in-place using Fisher-Yates shuffle algorithm.
        /// </summary>
        public void ShuffleFast<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Next(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }

    // Example usage in the context of your code
    public static class ListExtensions
    {
        public static void ShuffleFast<T>(this IList<T> list)
        {
            RandomGenerator.Instance.ShuffleFast(list);
        }
    }
}

using GAF;
using SLAPScheduling.Algorithm.DifferentialEvolutions;

namespace SLAPScheduling.Algorithm.Extensions
{
    public static class SolutionExt
    {
        public static Solution GetSolution(this Individual individual)
        {
            if (individual is not null && individual.Elements?.Count > 0)
            {
                var indices = individual.Elements.Select(x => (int)x).ToList();
                return new Solution(indices);
            }

            return new Solution();
        }

        public static Solution GetSolution(this Chromosome? chromosome)
        {
            if (chromosome is not null && chromosome.Genes?.Count > 0)
            {
                var indices = chromosome.Genes.Select(x => (int)x.RealValue).ToList();
                return new Solution(indices);
            }

            return new Solution();
        }

        /// <summary>
        /// Retrieve a list of locations from the solution
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public static List<Location> GetLocations(this Solution solution, Dictionary<int, Location> locationDictionary)
        {
            var locations = new List<Location>();
            foreach (var locationIndex in solution.Indices)
            {
                if (locationDictionary.TryGetValue(locationIndex, out Location? location))
                {
                    locations.Add(location);
                }
            }

            return locations;
        }
    }
}

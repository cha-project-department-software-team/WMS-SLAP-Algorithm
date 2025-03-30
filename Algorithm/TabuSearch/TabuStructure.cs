namespace SLAPScheduling.Algorithm.TabuSearch
{
    /// <summary>
    /// Tabu Structure is used for storing all Solutions and their object values in each iteration.
    /// </summary>
    public class TabuStructure
    {
        private Dictionary<Solution, double> solutionValues { get; set; }

        #region Constructor
        public TabuStructure(IEnumerable<Solution> solutions)
        {
            solutionValues = solutions.ToDictionary(solution => solution, objectValue => 0.0);
        }

        #endregion

        #region Retrieve Solution

        /// <summary>
        /// Retrieve list Solutions in this Tabu Structure
        /// </summary>
        /// <returns></returns>
        public List<Solution>? GetSolutions()
        {
            if (solutionValues?.Count > 0)
            {
                return solutionValues.Keys.ToList();
            }

            return null;
        }

        /// <summary>
        /// Retrieve the object value from Solution
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public double GetObjectValue(Solution solution)
        {
            return solutionValues.TryGetValue(solution, out double value) ? value : 0.0;
        }

        /// <summary>
        /// Retrieve the Solution having the best object value from Tabu Structure
        /// </summary>
        /// <returns></returns>
        public (Solution Solution, double ObjectValue) GetBestSolution()
        {
            var bestSolution = solutionValues.Aggregate((l, r) => l.Value < r.Value ? l : r);
            return (bestSolution.Key, bestSolution.Value);
        }

        #endregion

        #region Update Object Value for Solution

        /// <summary>
        /// Update the object value for Solution
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="value"></param>
        public void UpdateObjectValue(Solution solution, double value)
        {
            if (solutionValues.ContainsKey(solution))
            {
                solutionValues[solution] = value;
            }
        }

        #endregion
    }
}

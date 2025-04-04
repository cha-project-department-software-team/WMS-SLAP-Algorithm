using System.Numerics;

namespace SLAPScheduling.Algorithm.TabuSearch
{
    /// <summary>
    /// Tabu Structure is used for storing all Solutions and their object values in each iteration.
    /// </summary>
    public class TabuStructure
    {
        private Dictionary<Solution, double> solutionValues { get; set; }

        #region Constructor
        public TabuStructure(Solution bestSolution)
        {
            solutionValues = new Dictionary<Solution, double>(new SolutionComparer());

            var candidateSolutions = GetCandidateSolutions(bestSolution);
            if (candidateSolutions?.Count() > 0)
            {
                foreach (var solution in candidateSolutions)
                {
                    if (!solutionValues.ContainsKey(solution))
                    {
                        solutionValues.Add(solution, 0.0);
                    }
                }
            }
        }

        /// <summary>
        /// Swap all two neighbor elements in the best solution (from last iteration) to get the list of candidate solutions.
        /// </summary>
        /// <param name="currentSolution"></param>
        /// <returns></returns>
        private IEnumerable<Solution> GetCandidateSolutions(Solution currentSolution)
        {
            for (int i = 0; i < currentSolution.Indices.Count - 1; i++)
            {
                Solution? candidateSolution = currentSolution.SwapSolution(currentSolution.Indices[i], currentSolution.Indices[i + 1]);
                if (candidateSolution != null)
                {
                    yield return candidateSolution;
                }
            }
        }

        #endregion

        #region Retrieve Solution

        /// <summary>
        /// Retrieve list Solutions in this Tabu Structure
        /// </summary>
        /// <returns></returns>
        public List<Solution> GetSolutions()
        {
            return solutionValues.Keys.ToList();
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

        /// <summary>
        /// Find another solution which is not existing in tabu list.
        /// </summary>
        /// <param name="existedSolutions"></param>
        /// <returns></returns>
        public (Solution Solution, double ObjectValue) GetOtherBestSolution(TabuList tabuList)
        {
            var remainSolutions = solutionValues.Where(x => !tabuList.IsExist(x.Key));
            var bestSolution = remainSolutions.Aggregate((l, r) => l.Value < r.Value ? l : r);
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

    public class SolutionComparer : IEqualityComparer<Solution>
    {
        public bool Equals(Solution thisSolution, Solution thatSolution)
        {
            return thisSolution.Indices.SequenceEqual(thatSolution.Indices);
        }

        public int GetHashCode(Solution thisSolution)
        {
            return thisSolution.Indices.GetHashCode();
        }
    }
}

using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations;

namespace SLAPScheduling.Algorithm.TabuSearch
{
    /// <summary>
    /// Tabu List is used for preventing the same solution to be selected in the next iteration.
    /// </summary>
    public class TabuList
    {
        private static int tabuListLength { get; set; }
        public Stack<Solution> Solutions { get; private set; }

        #region Constructors
        public TabuList()
        {
            Solutions = new Stack<Solution>();
        }

        #endregion

        #region Determine the Length of Tabu List

        /// <summary>
        /// Determine the length of Tabu List
        /// </summary>
        /// <returns></returns>
        public static void InitializeTabuListLength(List<Location> locations)
        {
            if (locations?.Count > 0)
            {
                switch (locations.Count)
                {
                    case < 10:
                        tabuListLength = 2;
                        break;
                    case < 20:
                        tabuListLength = 5;
                        break;
                    default:
                        tabuListLength = 31;
                        break;
                }
            }
        }

        #endregion

        #region Retrieve Methods
        /// <summary>
        /// Retrieve all solutions in Tabu List as a List
        /// </summary>
        /// <returns></returns>
        public List<Solution> GetSolutions()
        {
            return Solutions?.Count > 0 ? Solutions.ToList() : new List<Solution>();
        }

        #endregion

        #region Add Solution to Tabu List
        /// <summary>
        /// Add the best solution in each iteration to Tabu List
        /// </summary>
        /// <param name="solution"></param>
        public void AddSolution(Solution solution)
        {
            if (IsOverTabuListLength())
            {
                Solutions.Pop();
            }

            if (Solutions != null && solution != null)
            {
                Solutions.Push(solution);
            }
        }

        /// <summary>
        /// Limit the length of Tabu List is lower than Tabu List Length
        /// </summary>
        /// <returns></returns>
        private bool IsOverTabuListLength()
        {
            return Solutions?.Count > tabuListLength;
        }

        #endregion

        #region Check if Solution is Exist in Tabu List

        /// <summary>
        /// Check the Solution is existed in Tabu List
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public bool IsExist(Solution solution)
        {
            return Solutions?.Count > 0 && Solutions.Any(x => x.Equals(solution));
        }

        #endregion
    }
}

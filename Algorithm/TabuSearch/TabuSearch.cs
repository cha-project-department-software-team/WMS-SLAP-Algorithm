//using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
//using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations;
//using System.Data;

//namespace SLAPScheduling.Algorithm.TabuSearch
//{
//    public class TabuSearch
//    {
//        private static int iterations => 100;
//        private List<ReceiptSublot> receiptSubLots { get; set; }
//        private Dictionary<int, Location> locationDictionary { get; set; }
//        private Solution bestSolution { get; set; }
//        private double bestObjectValue { get; set; }
//        private List<double> bestObjectValues { get; set; }
//        private TabuList tabuList { get; set; }

//        #region Constructor
//        public TabuSearch(List<ReceiptSublot> receiptSubLots, List<Location> availableLocations)
//        {
//            bestSolution = new Solution();
//            tabuList = new TabuList();
//            bestObjectValues = new List<double>();

//            this.receiptSubLots = new List<ReceiptSublot>();
//            locationDictionary = new Dictionary<int, Location>();
//            if (availableLocations?.Count > 0)
//            {
//                this.receiptSubLots = Enumerable.Range(0, availableLocations.Count).Select(index =>
//                {
//                    return index < receiptSubLots.Count ? receiptSubLots[index] : new ReceiptSublot();
//                }).ToList();

//                locationDictionary = Enumerable.Range(0, availableLocations.Count).ToDictionary(index => index, index => availableLocations[index]);
//                TabuList.InitializeTabuListLength(availableLocations);
//            }
//        }

//        #endregion

//        #region Implementation
//        /// <summary>
//        /// The main method for implementing the Tabu Search algorithm
//        /// </summary>
//        public List<Location> Implement()
//        {
//            bestSolution = InitialSolution();
//            bestObjectValue = CalculateObjectValue(bestSolution);

//            int terminate = 0;
//            while (terminate < iterations)
//            {
//                var candidateSolutions = GetCandidateSolutions(this.bestSolution);
//                TabuStructure tabuStructure = new TabuStructure(candidateSolutions);

//                // For each candidate solution, calculate the object value and update the tabu structure
//                foreach (var candidateSolution in candidateSolutions)
//                {
//                    double objectValue = CalculateObjectValue(candidateSolution);
//                    tabuStructure.UpdateObjectValue(candidateSolution, objectValue);
//                }

//                var (bestSolution, bestObjectValue) = tabuStructure.GetBestSolution();
//                if (bestObjectValue <= this.bestObjectValue && !tabuList.IsExist(bestSolution))
//                {
//                    tabuList.AddSolution(bestSolution);
//                    this.bestSolution = bestSolution;
//                    this.bestObjectValue = bestObjectValue;
//                }

//                bestObjectValues.Add(this.bestObjectValue);
//                terminate++;
//            }

//            var optimalLocations = GetLocations(bestSolution);
//            return optimalLocations?.Count() > 0 ? optimalLocations.ToList() : new List<Location>();
//        }

//        /// <summary>
//        /// Initial the solution as an index array of available locations.
//        /// </summary>
//        /// <returns></returns>
//        private Solution InitialSolution()
//        {
//            // Initialize the solution as the indices of locations based on the distance to I/O point
//            var sortedLocations = locationDictionary.OrderBy(x => x.Value.GetDistanceToIOPoint());
//            var initialIndices = sortedLocations.Select(x => x.Key).ToList();
//            return initialIndices?.Count > 0 ? new Solution(initialIndices) : new Solution();
//        }

//        /// <summary>
//        /// Swap all two neighbor elements in the best solution (from last iteration) to get the list of candidate solutions.
//        /// </summary>
//        /// <param name="currentSolution"></param>
//        /// <returns></returns>
//        private IEnumerable<Solution> GetCandidateSolutions(Solution currentSolution)
//        {
//            for (int i = 0; i < currentSolution.Indices.Count - 1; i++)
//            {
//                Solution? candidateSolution = currentSolution.SwapSolution(currentSolution.Indices[i], currentSolution.Indices[i + 1]);
//                if (candidateSolution != null)
//                {
//                    yield return candidateSolution;
//                }
//            }
//        }

//        /// <summary>
//        /// Retrieve a list of locations from the solution
//        /// </summary>
//        /// <param name="solution"></param>
//        /// <returns></returns>
//        private IEnumerable<Location> GetLocations(Solution solution)
//        {
//            foreach (var locationIndex in solution.Indices)
//            {
//                if (locationDictionary.TryGetValue(locationIndex, out Location? location))
//                {
//                    yield return location;
//                }
//            }
//        }

//        #endregion

//        #region Calculate Object Value

//        /// <summary>
//        /// Calculate the Object Value for the current solution
//        /// </summary>
//        /// <param name="currentSolution"></param>
//        /// <returns></returns>
//        private double CalculateObjectValue(Solution currentSolution)
//        {
//            if (currentSolution?.Indices?.Count != locationDictionary.Count)
//                return 0.0;

//            var objectValue = 0.0;
//            for (int index = 0; index < currentSolution.Indices.Count; index++)
//            {
//                var (location, receiptSubLot) = MapSolutionToSubLot(currentSolution, index);
//                if (location.IsValid() && receiptSubLot.IsValid())
//                {
//                    double penalty = !CheckConstraints(currentSolution) ? double.MaxValue : 0;
//                    objectValue += receiptSubLot.Material.GetMovementRatio() * location.GetDistanceToIOPoint() + penalty;
//                }
//            }

//            return objectValue;
//        }

//        #endregion

//        #region Constraints Checking
//        /// <summary>
//        /// Check the constraints for the current solution is satisfied or not
//        /// </summary>
//        /// <param name="solution"></param>
//        /// <returns></returns>
//        private bool CheckConstraints(Solution solution)
//        {
//            return !IsNotSatisfyStorageLevel(solution) && !IsOverStorageVolume(solution);
//        }

//        /// <summary>
//        /// A product must be stored in the level that is less than or equal to the limit storage level
//        /// </summary>
//        /// <param name="solution"></param>
//        /// <returns></returns>
//        private bool IsNotSatisfyStorageLevel(Solution solution)
//        {
//            for (int index = 0; index < solution.Indices.Count; index++)
//            {
//                var (location, receiptSubLot) = MapSolutionToSubLot(solution, index);
//                if (location.IsValid() && receiptSubLot.IsValid())
//                {
//                    if (location.GetLevelIndex() > receiptSubLot.Material.GetLimitStorageLevel())
//                        return false;
//                }
//            }

//            return true;
//        }

//        /// <summary>
//        /// After assigning receipt sublots to locations, the storage volume must not exceed the maximum storage volume
//        /// </summary>
//        /// <param name="solution"></param>
//        /// <returns></returns>
//        private bool IsOverStorageVolume(Solution solution)
//        {
//            var storagePercentage = 0.0;
//            for (int index = 0; index < solution.Indices.Count; index++)
//            {
//                var (location, receiptSubLot) = MapSolutionToSubLot(solution, index);
//                if (location.IsValid() && receiptSubLot.IsValid())
//                {
//                    storagePercentage += location.GetCurrentStoragePercentage() + location.GetStoragePercentage(receiptSubLot);
//                    if (storagePercentage > 1.0)
//                        return true;
//                }
//            }

//            return false;
//        }

//        #endregion

//        #region Mapping Solution to Receipt Sublot

//        /// <summary>
//        /// Use the index of the solution for mapping to the Receipt Sublot
//        /// </summary>
//        /// <param name="solution"></param>
//        /// <param name="index"></param>
//        /// <returns></returns>
//        public (Location? Location, ReceiptSublot? SubLot) MapSolutionToSubLot(Solution solution, int index)
//        {
//            if (solution != null)
//            {
//                var receiptSubLot = receiptSubLots[index];
//                var locationIndex = solution.Indices[index];
//                if (locationDictionary.TryGetValue(locationIndex, out Location? location))
//                {
//                    return (location, receiptSubLot);
//                }
//            }

//            return (null, null);
//        }

//        #endregion
//    }
//}

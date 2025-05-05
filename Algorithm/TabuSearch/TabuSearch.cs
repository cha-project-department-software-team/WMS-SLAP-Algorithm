using System.Data;
using System.Diagnostics;

namespace SLAPScheduling.Algorithm.TabuSearch
{
    public class TabuSearch
    {
        private static int iterations => 20000;
        private List<ReceiptSublot> receiptSubLots { get; set; }
        private Dictionary<int, Location> locationDictionary { get; set; }
        private Solution bestSolution { get; set; }
        private double bestObjectValue { get; set; }
        private List<double> bestObjectValues { get; set; }
        private List<(double Time, double ObjectValue)> timeChangeObjectValues { get; set; }
        private List<(int SolutionCount, double ObjectValue)> solutionChangeObjectValues { get; set; }
        private TabuList tabuList { get; set; }
        private int numberOfReceiptSubLots { get; set; }
        private int numberOfEvaluatedSolutions { get; set; }

        #region Constructor
        public TabuSearch(List<ReceiptSublot> receiptSubLots, List<Location> availableLocations)
        {
            bestSolution = new Solution();
            tabuList = new TabuList();
            bestObjectValues = new List<double>();
            timeChangeObjectValues = new List<(double, double)>();
            solutionChangeObjectValues = new List<(int SolutionCount, double ObjectValue)>();

            this.receiptSubLots = new List<ReceiptSublot>();
            locationDictionary = new Dictionary<int, Location>();
            if (availableLocations?.Count > 0)
            {
                this.numberOfReceiptSubLots = receiptSubLots.Count;
                this.numberOfEvaluatedSolutions = 0;

                this.receiptSubLots = Enumerable.Range(0, availableLocations.Count).Select(index =>
                {
                    return index < receiptSubLots.Count ? receiptSubLots[index] : new ReceiptSublot();
                }).ToList();

                locationDictionary = Enumerable.Range(0, availableLocations.Count).ToDictionary(index => index, index => availableLocations[index]);
                TabuList.InitializeTabuListLength(availableLocations);
            }
        }

        #endregion

        #region Update Evaluated Solutions

        private void UpdateEvaluatedSolution(int evaluatedSolution, double currentObjectValue)
        {
            this.numberOfEvaluatedSolutions += evaluatedSolution;
            this.solutionChangeObjectValues.Add((this.numberOfEvaluatedSolutions, currentObjectValue));
        }

        #endregion

        #region Implementation
        /// <summary>
        /// The main method for implementing the Tabu Search algorithm
        /// </summary>
        public List<Location> Implement()
        {
            Stopwatch sw = Stopwatch.StartNew();

            bestSolution = InitialSolution();
            bestObjectValue = bestSolution.CalculateObjectValue(receiptSubLots, locationDictionary);
            UpdateEvaluatedSolution(1, bestObjectValue);

            int terminate = 0;
            while (terminate < iterations)
            {
                TabuStructure tabuStructure = new TabuStructure(this.bestSolution);

                // For each candidate solution, calculate the object value and update the tabu structure
                foreach (var candidateSolution in tabuStructure.GetSolutions())
                {
                    double objectValue = candidateSolution.CalculateObjectValue(receiptSubLots, locationDictionary);
                    tabuStructure.UpdateObjectValue(candidateSolution, objectValue);
                }

                var (bestSolution, bestObjectValue) = tabuStructure.GetBestSolution();
                
                if (bestObjectValue <= this.bestObjectValue && !this.tabuList.IsExist(bestSolution))
                {
                    this.tabuList.AddSolution(bestSolution);
                    this.bestSolution = bestSolution;
                    this.bestObjectValue = bestObjectValue;
                }
                else
                {
                    // Best Solution is existed in Tabu List, find another solution.
                    var (otherBestSolution, otherBestObjectValue) = tabuStructure.GetOtherBestSolution(this.tabuList);
                    if (!tabuList.IsExist(otherBestSolution))
                    {
                        this.tabuList.AddSolution(otherBestSolution);
                        this.bestSolution = otherBestSolution;
                        this.bestObjectValue = otherBestObjectValue;
                    }
                }

                bestObjectValues.Add(this.bestObjectValue);
                UpdateEvaluatedSolution(tabuStructure.GetSolutionCount(), bestObjectValue);
                timeChangeObjectValues.Add((sw.Elapsed.TotalSeconds, this.bestObjectValue));

                terminate++;
            }

            sw.Stop();

            //using (TextWriter writer = File.CreateText(@"C:\Users\AnhTu\Master Subjects\Luan van Thac si\Document\SchedulingResult.json"))
            //{
            //    var serializer = new JsonSerializer();
            //    serializer.Serialize(writer, timeChangeObjectValues);
            //}

            var optimalLocations = bestSolution.GetLocations(locationDictionary);
            return optimalLocations?.Count() > 0 ? optimalLocations.ToList() : new List<Location>();
        }

        /// <summary>
        /// Initial the solution as an index array of available locations.
        /// </summary>
        /// <returns></returns>
        private Solution InitialSolution()
        {
            // The initial solution has a huge impact on the finding for optimal solution, so it must be chosen appropriate.
            var groupedLocations = locationDictionary.GroupBy(loc => loc.Value.GetStorageLevel())
                                    .Select(group => new
                                    {
                                        Index = group.Key,
                                        SortedLocations = group.OrderBy(loc => loc.Value.GetCurrentStoragePercentage()).ToList()
                                    }).ToList();

            var initialIndices = groupedLocations.SelectMany(x => x.SortedLocations.Select(x => x.Key)).ToList();

            //var sortedLocations = locationDictionary.OrderBy(x => x.Value.GetStorageLevel());
            //var initialIndices = sortedLocations.Select(x => x.Key).ToList();

            return initialIndices?.Count > 0 ? new Solution(initialIndices) : new Solution();
        }

        #endregion
    }
}

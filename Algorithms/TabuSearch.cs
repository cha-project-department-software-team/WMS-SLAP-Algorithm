using TabuSearchProductionScheduling.Classes;
using TabuSearchProductionScheduling.Planning;

namespace TabuSearchProductionScheduling.Algorithms
{
    public class TabuSearch
    {
        private static int iterations => 100;
        private List<WorkOrder> workOrders { get; set; }
        private DateTime firstDateStart {  get; set; }
        public double BestObjectValue { get; set; }
        public List<int> BestSolution { get; set; }

        public TabuSearch(List<WorkOrder> workOrders, DateTime firstDateStart)
        {
            this.workOrders = workOrders;
            this.firstDateStart = firstDateStart;
        }

        private int GetTenure()
        {
            int numberOfWorks = workOrders.Count;
            if (numberOfWorks < 10)
                return 2;
            else if (numberOfWorks < 20)
                return 5;
            else 
                return 31;
        }

        private List<int> InitialSolution()
        {
            List<int> solution = new List<int>();
            foreach (WorkOrder work in workOrders)
            {
                solution.Add(work.Id);
            }

            return solution;
        }

        private static Dictionary<(int Index1, int Index2), double> TabuStructure(List<int> solution)
        {
            Dictionary<(int Index1, int Index2), double> tabuAttribute = new Dictionary<(int Index1, int Index2), double>();
            for (int i = 0; i < solution.Count - 1; i++)
            {
                (int Index1, int Index2) pair = (solution[i], solution[i + 1]);
                tabuAttribute.Add(pair, 0);
            }

            return tabuAttribute;
        }

        private List<int> SwapPairs(List<int> solution, int i, int j)
        {
            try
            {
                int iIndex = solution.IndexOf(i);
                int jIndex = solution.IndexOf(j);
                int temp = solution[iIndex];
                solution[iIndex] = solution[jIndex];
                solution[jIndex] = temp;
                return solution;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return solution;
        }

        private static bool CheckTabuList((int Index1, int Index2) bestPair, List<(int Index1, int Index2)> tabuList)
        {
            for (int i = 0; i < tabuList.Count; i++)
            {
                if ((bestPair.Index1 == tabuList[i].Index1) && (bestPair.Index2 == tabuList[i].Index2))
                    return true;
            }

            return false;
        }

        private static List<(int Index1, int Index2)> UpdateTabuList((int Index1, int Index2) bestMove, List<(int Index1, int Index2)> tabuList, int tenure)
        {
            if (tabuList.Count < tenure)
                tabuList.Add(bestMove);
            else
            {
                for (int i = 0; i < (tabuList.Count - 1); i++)
                {
                    tabuList[i] = tabuList[i + 1];
                }

                tabuList[tabuList.Count - 1] = bestMove;
            }

            return tabuList;
        }

        private (int Index1, int Index2) GetBestPair(Dictionary<(int Index1, int Index2), double> tabuAttribute, List<(int Index1, int Index2)> tabuList, int tenure)
        {
            (int Index1, int Index2) bestPair = (0, 0);
            //Find the minimum Object Value in Tabu Attribute dictionary
            var minObjectValue = tabuAttribute.Min(x => x.Value);

            //There can be many pairs with the same minimum value
            var minObjectPairs = new List<(int Index1, int Index2)>();
            foreach (var kvp in tabuAttribute)
            {
                if (kvp.Value == minObjectValue)
                    minObjectPairs.Add(kvp.Key);
            }

            if (minObjectPairs.Count > 0)
            {
                foreach (var pair in minObjectPairs)
                {
                    if (!CheckTabuList(bestPair, tabuList))
                    {
                        bestPair = pair;
                        tabuList = UpdateTabuList(bestPair, tabuList, tenure);
                        break;
                    }
                }
            }

            return bestPair;
        }

        private double GetObjectValue(List<int> solution)
        {
            double objectValue = 0;
            WorkOrderPlanning planning = new WorkOrderPlanning(firstDateStart);
            foreach (int job in solution)
            {
                var workOrder = workOrders.FirstOrDefault(x => x.Id == job);
                if (workOrder == null)
                    continue;

                workOrder = planning.Planning(workOrder);
                if (workOrder.EndDate > workOrder.DueDate)
                    objectValue += workOrder.Priority;
            }

            return objectValue;
        }

        private List<int> GetBestSolution()
        {
            int tenure = GetTenure();
            List<(int Index1, int Index2)> tabuList = new List<(int Index1, int Index2)>();
            List<int> currentSolution = InitialSolution();

            BestObjectValue = GetObjectValue(currentSolution);
            BestSolution = currentSolution;

            var bestValues = new List<double>();
            int terminate = 0;
            while (terminate < iterations)
            {
                var tabuStructure = TabuStructure(BestSolution);
                foreach(var kvp in tabuStructure)
                {
                    var pair = kvp.Key; 
                    var candidateSolution = SwapPairs(BestSolution, pair.Index1, pair.Index2);
                    tabuStructure[pair] = GetObjectValue(candidateSolution);
                }

                var bestPair = GetBestPair(tabuStructure, tabuList, tenure);
                if (bestPair.Index1 != 0 && bestPair.Index2 != 0)
                {
                    currentSolution = SwapPairs(BestSolution, bestPair.Index1, bestPair.Index2);
                    double currentValue = GetObjectValue(currentSolution);

                    if (currentValue < BestObjectValue)
                    {
                        BestSolution = currentSolution;
                        BestObjectValue = currentValue;
                    }
                }

                bestValues.Add(BestObjectValue);
                terminate += 1;
            }

            return BestSolution;
        }

        public List<WorkOrder> GetScheduledWorkOrders()
        {
            var scheduledWorkOrders = new List<WorkOrder>();
            var workOrderDictionary = this.workOrders.ToDictionary(x => x.Id, x => x);

            List<int> bestSolution = GetBestSolution();
            foreach (var index in bestSolution)
            {
                if (workOrderDictionary.ContainsKey(index))
                    scheduledWorkOrders.Add(workOrderDictionary[index]);
            }

            return scheduledWorkOrders;

        }
    }
}

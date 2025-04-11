namespace SLAPScheduling.Algorithm.ObjectValue
{
    public static class ObjectValueCalculator
    {
        #region Calculate Object Value

        /// <summary>
        /// Calculate the Object Value for the current solution
        /// </summary>
        /// <param name="currentSolution"></param>
        /// <returns></returns>
        public static double CalculateObjectValue(this Solution currentSolution, List<ReceiptSublot> receiptSubLots, Dictionary<int, Location> locationDictionary)
        {
            if (currentSolution?.Indices?.Count != locationDictionary.Count)
                return 0.0;

            var objectValue = 0.0;
            for (int index = 0; index < currentSolution.Indices.Count; index++)
            {
                var (location, receiptSubLot) = MapSolutionToSubLot(currentSolution, receiptSubLots, locationDictionary, index);
                if (location is not null && receiptSubLot is not null)
                {
                    var material = receiptSubLot.GetMaterial();
                    if (material is not null)
                    {
                        double penalty = location.CalculatePenaltyValue(receiptSubLot);
                        objectValue += material.GetMovementRatio() * location.GetDistanceToIOPoint() + penalty;
                    }
                }
            }

            return objectValue;
        }

        #endregion

        #region Mapping Solution to Receipt Sublot

        /// <summary>
        /// Use the index of the solution for mapping to the Receipt Sublot
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static (Location? Location, ReceiptSublot? SubLot) MapSolutionToSubLot(Solution solution, List<ReceiptSublot> receiptSubLots, Dictionary<int, Location> locationDictionary, int index)
        {
            if (solution != null)
            {
                var receiptSubLot = receiptSubLots[index];
                var locationIndex = solution.Indices[index];
                if (locationDictionary.TryGetValue(locationIndex, out Location? location))
                {
                    return (location, receiptSubLot);
                }
            }

            return (null, null);
        }

        #endregion
    }
}

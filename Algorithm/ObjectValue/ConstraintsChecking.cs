namespace SLAPScheduling.Algorithm.ObjectValue
{
    public static class ConstraintsChecking
    {
        private static double levelCoefficient { get; set; }
        private static double storageCoefficient { get; set; }

        #region Constraints Checking
        /// <summary>
        /// Choose the values of penalty coefficient from list of locations
        /// </summary>
        /// <param name="locations"></param>
        public static void SetPenaltyCoefficientValues(IEnumerable<Location> locations)
        {
            levelCoefficient = locations.Max(x => x.GetDistanceToIOPoint());
            storageCoefficient = locations.Sum(x => x.GetDistanceToIOPoint());
        }

        /// <summary>
        /// Calculate the penalty value based on the checking the storage constraints
        /// </summary>
        /// <param name="location"></param>
        /// <param name="receiptSublot"></param>
        /// <returns></returns>
        public static double CalculatePenaltyValue(this Location location, ReceiptSublot receiptSublot)
        {
            double overStorageLevel = CheckSatisfyStorageLevel(location, receiptSublot);
            double overStoragePercentage = CheckOverStorageVolume(location, receiptSublot);

            return levelCoefficient * Math.Max(0, overStorageLevel) + storageCoefficient * Math.Max(0, overStoragePercentage);
        }

        /// <summary>
        /// Check the constraints for the current solution is satisfied or not
        /// </summary>
        /// <param name="location"></param>
        /// <param name="receiptSublot"></param>
        /// <returns></returns>
        public static bool CheckStorageConstraints(this Location location, ReceiptSublot receiptSublot)
        {
            return CheckSatisfyStorageLevel(location, receiptSublot) + CheckOverStorageVolume(location, receiptSublot) > 0.0;
        }

        /// <summary>
        /// A product must be stored in the level that is less than or equal to the limit storage level
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public static int CheckSatisfyStorageLevel(Location location, ReceiptSublot receiptSublot)
        {
            int overStorageLevel = 0;
            if (location is not null && receiptSublot is not null)
            {
                var material = receiptSublot.GetMaterial();
                if (material is not null && location.GetStorageLevel() > material.GetLimitStorageLevel())
                {
                    overStorageLevel = location.GetStorageLevel() - material.GetLimitStorageLevel();
                }
            }

            return overStorageLevel;
        }

        /// <summary>
        /// After assigning receipt sublots to locations, the storage volume must not exceed the maximum storage volume
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public static double CheckOverStorageVolume(Location location, ReceiptSublot receiptSublot)
        {
            double overStoragePercentage = 0;
            if (location is not null && receiptSublot is not null)
            {
                double currentLocationStorage = location.GetCurrentStoragePercentage();
                double subLotStorage = location.GetStoragePercentage(receiptSublot);

                double storagePercentage = currentLocationStorage + subLotStorage;
                if (storagePercentage > 1.0)
                {
                    overStoragePercentage = storagePercentage - 1.0;
                }
            }

            return overStoragePercentage;
        }

        #endregion
    }
}

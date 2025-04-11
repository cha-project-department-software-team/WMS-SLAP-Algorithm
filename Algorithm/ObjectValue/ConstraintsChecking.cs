namespace SLAPScheduling.Algorithm.ObjectValue
{
    public static class ConstraintsChecking
    {
        #region Constraints Checking

        /// <summary>
        /// Calculate the penalty value based on the checking the storage constraints
        /// </summary>
        /// <param name="location"></param>
        /// <param name="receiptSublot"></param>
        /// <returns></returns>
        public static double CalculatePenaltyValue(this Location location, ReceiptSublot receiptSublot)
        {
            double penalty = 0.0;
            if (IsNotSatisfyStorageLevel(location, receiptSublot))
            {
                penalty += 1000;
            }

            if (IsOverStorageVolume(location, receiptSublot))
            {
                penalty += 2000;
            }

            return penalty;
        }

        /// <summary>
        /// Check the constraints for the current solution is satisfied or not
        /// </summary>
        /// <param name="location"></param>
        /// <param name="receiptSublot"></param>
        /// <returns></returns>
        public static bool CheckStorageConstraints(this Location location, ReceiptSublot receiptSublot)
        {
            return !IsNotSatisfyStorageLevel(location, receiptSublot) && !IsOverStorageVolume(location, receiptSublot);
        }

        /// <summary>
        /// A product must be stored in the level that is less than or equal to the limit storage level
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        private static bool IsNotSatisfyStorageLevel(Location location, ReceiptSublot receiptSublot)
        {
            if (location is not null && receiptSublot is not null)
            {
                var material = receiptSublot.GetMaterial();
                if (material is not null && location.GetStorageLevel() > material.GetLimitStorageLevel())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// After assigning receipt sublots to locations, the storage volume must not exceed the maximum storage volume
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        private static bool IsOverStorageVolume(Location location, ReceiptSublot receiptSublot)
        {
            if (location is not null && receiptSublot is not null)
            {
                double currentLocationStorage = location.GetCurrentStoragePercentage();
                double subLotStorage = location.GetStoragePercentage(receiptSublot);

                double storagePercentage = currentLocationStorage + subLotStorage;
                if (storagePercentage > 1.0)
                    return true;
            }

            return false;
        }

        #endregion
    }
}

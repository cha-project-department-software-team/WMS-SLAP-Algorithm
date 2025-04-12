using SLAPScheduling.Algorithm.ObjectValue;

namespace SLAPScheduling.Algorithm.Helpers
{
    public class ReceiptSublotReallocation
    {
        private List<Location> allocatedLocations { get; set; }

        public ReceiptSublotReallocation(List<Location> locations, List<ReceiptSublot> receiptSublots)
        {
            this.allocatedLocations = new List<Location>();
            for (int i = 0; i < receiptSublots.Count; i++)
            {
                // Update Receipt Sublot to location
                locations[i].AddReceiptSublot(receiptSublots[i]);
                this.allocatedLocations.Add(locations[i]);
            }
        }

        /// <summary>
        /// Implement the logic of allocating receipt sublots
        /// </summary>
        /// <returns></returns>
        public List<(ReceiptSublot SubLot, double StoragePercentage)> Implement()
        {
            var results = new List<(ReceiptSublot SubLot, double StoragePercentage)>();
            if (this.allocatedLocations is not null)
            {
                var orderedLocations = this.allocatedLocations.OrderByDescending(x => x.GetDistanceToIOPoint()).ToList();
                ReallocateReceiptSublots(ref orderedLocations);

                foreach (var location in orderedLocations)
                {
                    if (location.receiptSublots is null || location.receiptSublots.Count == 0)
                        continue;

                    foreach (var receiptSublot in location.receiptSublots)
                    {
                        receiptSublot.UpdateLocation(location);
                        var storagePercentage = location.GetStoragePercentage(receiptSublot);

                        results.Add((receiptSublot, storagePercentage));
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Re-allocate the location of receipt sublots
        /// </summary>
        /// <param name="locations"></param>
        private void ReallocateReceiptSublots(ref List<Location> locations)
        {
            var locationCount = locations.Count;
            for (int i = locationCount - 1; i >= 0; i--)
            {
                var location = locations[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    var nextLocation = locations[j];
                    var suitableSublots = GetSuitableReceiptSublots(location, nextLocation);
                    foreach (var suitableSublot in suitableSublots)
                    {
                        if (nextLocation.RemoveReceiptSubLot(suitableSublot))
                        {
                            location.AddReceiptSublot(suitableSublot);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="receiptSublots"></param>
        /// <returns></returns>
        private List<ReceiptSublot> GetSuitableReceiptSublots(Location location, Location nextLocation)
        {
            var suitableSublots = new List<ReceiptSublot>();

            var receiptSublots = nextLocation.GetReceiptSublots();
            if (receiptSublots?.Count > 0)
            {
                var currentStorage = location.GetReceiptAndMaterialStoragePercentage() + suitableSublots.Sum(sublot => location.GetStoragePercentage(sublot));
                foreach (var receiptSublot in receiptSublots)
                {
                    var sublotStorage = location.GetStoragePercentage(receiptSublot);
                    if (currentStorage + sublotStorage < 1.0 && location.CheckStorageConstraints(receiptSublot))
                    {
                        suitableSublots.Add(receiptSublot);
                    }
                }
            }

            return suitableSublots;
        }
    }
}

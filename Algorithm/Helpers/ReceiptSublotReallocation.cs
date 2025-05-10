namespace SLAPScheduling.Algorithm.Helpers
{
    public class ReceiptSublotReallocation
    {
        private List<Location> allocatedLocations { get; set; }

        public ReceiptSublotReallocation()
        {
            this.allocatedLocations = new List<Location>();
        }

        /// <summary>
        /// Implement the logic of allocating receipt sublots
        /// </summary>
        /// <returns></returns>
        public List<(ReceiptSublot SubLot, double StoragePercentage)>? Reallocate(List<Location> locations, List<ReceiptSublot> receiptSublots)
        {
            var assignedLocations = AssignReceiptSubLotsForLocations(locations, receiptSublots);
            this.allocatedLocations = assignedLocations.OrderBy(location => location.GetDistanceToIOPoint()).ToList();
            for (int index = 0; index < this.allocatedLocations.Count; index++)
            {
                var location = this.allocatedLocations[index];
                var storagePercent = location.GetReceiptAndMaterialStoragePercentage();
                if (storagePercent <= 0.5f)
                {
                    var subLots = location.GetReceiptSublots();
                    if (subLots?.Count > 0)
                    {
                        var addedSubLots = new List<ReceiptSublot>();
                        for (int replaceIndex = 0; replaceIndex < index; replaceIndex++)
                        {
                            var replaceLocation = this.allocatedLocations[replaceIndex];
                            if (TryAddReceiptSublots(subLots, replaceLocation, out addedSubLots))
                            {
                                ReallocateReceiptSublots(addedSubLots, location, replaceLocation);
                                break;
                            }
                        }
                    }
                }
            }

            return GetReallocateResult();
        }

        private List<Location> AssignReceiptSubLotsForLocations(List<Location> locations, List<ReceiptSublot> receiptSublots)
        {
            var assignedLocations = new List<Location>();
            for (int i = 0; i < receiptSublots.Count; i++)
            {
                // Update Receipt Sublot to location
                locations[i].AddReceiptSublot(receiptSublots[i]);
                assignedLocations.Add(locations[i]);
            }

            return assignedLocations;
        }

        private bool TryAddReceiptSublots(List<ReceiptSublot> subLots, Location replaceLocation, out List<ReceiptSublot> addedSublots)
        {
            addedSublots = new List<ReceiptSublot>();
            foreach (var sublot in subLots)
            {
                var assignedStoragePercent = replaceLocation.GetReceiptAndMaterialStoragePercentage() 
                                           + sublot.GetStoragePercentage(replaceLocation) 
                                           + addedSublots.Sum(x => x.GetStoragePercentage(replaceLocation));

                if (assignedStoragePercent < 1.0f)
                {
                    if (!replaceLocation.IsStorageConstraintsViolated(sublot))
                    {
                        addedSublots.Add(sublot);
                    }
                }
            }

            return addedSublots?.Count > 0;
        }

        private void ReallocateReceiptSublots(List<ReceiptSublot> receiptSublots, Location fromLocation, Location toLocation)
        {
            if (receiptSublots?.Count > 0)
            {
                foreach (var sublot in receiptSublots)
                {
                    if (fromLocation.RemoveReceiptSubLot(sublot))
                    {
                        toLocation.AddReceiptSublot(sublot);
                    }
                }
            }
        }

        private List<(ReceiptSublot SubLot, double StoragePercentage)> GetReallocateResult()
        {
            var results = new List<(ReceiptSublot SubLot, double StoragePercentage)>();
            foreach (var location in this.allocatedLocations)
            {
                if (location.receiptSublots is null || location.receiptSublots.Count == 0)
                    continue;

                foreach (var receiptSublot in location.receiptSublots)
                {
                    receiptSublot.UpdateLocation(location);
                    var storagePercentage = receiptSublot.GetStoragePercentage(location);

                    results.Add((receiptSublot, storagePercentage));
                }
            }

            return results;
        }
    }
}

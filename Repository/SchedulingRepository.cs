using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.StorageAggregate;
using SLAPScheduling.AggregateModels.SchedulingAggregate;
using SLAPScheduling.Helpers;
using SLAPScheduling.TabuSearch;
using Material = SLAP.AggregateModels.MaterialAggregate.Material;

namespace SLAP.Repository
{
    public class SchedulingRepository : ISchedulingtRepository
    {
        public List<ReceiptSubLot> Implement(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials)
        {
            if (inventoryReceipt == null || warehouse == null || materials == null)
                return new List<ReceiptSubLot>();

            var receiptSubLots = new List<ReceiptSubLot>();
            using (var receiptLotSplitter = new ReceiptLotSplitter(inventoryReceipt.Entries, materials, warehouse))
            {
                // Receipt Sublots do not include the Location information
                receiptSubLots = receiptLotSplitter.GetReceiptSubLots().ToList();
            }

            // Order by descending based on the movement ratio of a product
            receiptSubLots = receiptSubLots.OrderByDescending(x => x.Material.GetMovementRatio()).ToList();

            // Retrieve the available locations (not full) in the warehouse
            var availableLocations = warehouse.Locations.Where(x => x.GetCurrentStoragePercentage() < 1.0);

            // Find the optimal solution of location assignment for each receipt sublot using Tabu Search algorithm
            TabuSearch tabuSearch = new TabuSearch(receiptSubLots, availableLocations.ToList());
            List<Location> optimalLocations = tabuSearch.Implement();

            UpdateLocationForReceiptSubLot(optimalLocations, ref receiptSubLots);
            return receiptSubLots;
        }

        private void UpdateLocationForReceiptSubLot(List<Location> locations, ref List<ReceiptSubLot> receiptSubLots)
        {
            if (receiptSubLots?.Count > 0 && locations?.Count > 0)
            {
                for (int i = 0; i < receiptSubLots.Count; i++)
                {
                    receiptSubLots[i].UpdateLocation(locations[i]);
                }
            }
        }
    }
}

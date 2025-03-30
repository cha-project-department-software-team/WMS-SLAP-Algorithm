using SLAPScheduling.Algorithm.Helpers;
using SLAPScheduling.Algorithm.TabuSearch;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;
using SLAPScheduling.Domain.InterfaceRepositories.IScheduling;
using Material = SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials.Material;

namespace SLAPScheduling.Infrastructure.Repository.Scheduling
{
    public class SchedulingRepository : BaseRepository, ISchedulingRepository
    {
        public SchedulingRepository(SLAPDbContext context) : base(context)
        {
        }

        /// <summary>
        /// POST API to implement the scheduling based on the input data.
        /// </summary>
        /// <param name="inventoryReceipt"></param>
        /// <param name="warehouse"></param>
        /// <param name="materials"></param>
        /// <returns></returns>
        public List<ReceiptSublot> Execute(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials)
        {
            if (inventoryReceipt == null || warehouse == null || materials == null)
                return new List<ReceiptSublot>();

            var receiptSubLots = new List<ReceiptSublot>();
            using (var receiptLotSplitter = new ReceiptLotSplitter(inventoryReceipt.Entries, materials, warehouse))
            {
                // Receipt Sublots do not include the Location information
                receiptSubLots = receiptLotSplitter.GetReceiptSubLots().ToList();
            }

            // Order by descending based on the movement ratio of a product
            receiptSubLots = receiptSubLots.OrderByDescending(x => x.Material.GetMovementRatio()).ToList();

            // Retrieve the available locations (not full) in the warehouse
            var availableLocations = warehouse.locations.Where(x => x.GetCurrentStoragePercentage() < 1.0);

            // Find the optimal solution of location assignment for each receipt sublot using Tabu Search algorithm
            TabuSearch tabuSearch = new TabuSearch(receiptSubLots, availableLocations.ToList());
            List<Location> optimalLocations = tabuSearch.Implement();

            UpdateLocationForReceiptSubLot(optimalLocations, ref receiptSubLots);
            return receiptSubLots;
        }

        /// <summary>
        /// Assign the Location to each ReceiptSubLot based on the optimal solution
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="receiptSubLots"></param>
        private void UpdateLocationForReceiptSubLot(List<Location> locations, ref List<ReceiptSublot> receiptSubLots)
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

using SLAP.AggregateModels.InventoryIssueAggregate;
using SLAP.AggregateModels.InventoryReceiptAggregate;

namespace SLAP.AggregateModels.StorageAggregate
{
    public class Warehouse
    {
        public string warehouseId { get; set; }
        public string warehouseName { get; set; }
        public List<Location> locations { get; set; }

        public Warehouse(string warehouseId, string warehouseName)
        {
            this.warehouseId = warehouseId;
            this.warehouseName = warehouseName;
        }
    }
}

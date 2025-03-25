using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAP.AggregateModels.StorageAggregate
{
    public class Location
    {
        public string locationId { get; set; }
        public List<MaterialSubLot> materialSubLots { get; set; }
        public List<ReceiptSublot> receiptSublots { get; set; }
        public string warehouseId { get; set; }
        public Warehouse warehouse { get; set; }

        public Location(string locationId, string warehouseId)
        {
            this.locationId = locationId;
            this.warehouseId = warehouseId;
        }
    }
}

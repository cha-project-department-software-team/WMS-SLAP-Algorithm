using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAP.AggregateModels.StorageAggregate
{
    public class Location
    {
        public string locationId { get; set; }
        public List<MaterialSubLot> materialSubLots { get; set; }
        public Warehouse warehouse { get; set; }

        public Location()
        {
        }

        public Location(string locationId, List<MaterialSubLot> materialSubLots, Warehouse warehouse)
        {
            this.locationId = locationId;
            this.materialSubLots = materialSubLots;
            this.warehouse = warehouse;
        }
    }
}

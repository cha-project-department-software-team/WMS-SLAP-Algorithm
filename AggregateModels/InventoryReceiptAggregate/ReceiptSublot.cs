using SLAP.AggregateModels.MaterialAggregate;
using SLAP.AggregateModels.StorageAggregate;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptSubLot
    {
        public string ReceiptSublotId { get; set; }
        public Material? Material { get; set; }
        public Location? Location { get; set; }
        public double ImportedQuantity { get; set; }

        public ReceiptSubLot()
        {
        }

        public ReceiptSubLot(string receiptSublotId, Material? material, Location? location, double importedQuantity)
        {
            ReceiptSublotId = receiptSublotId;
            Material = material;
            Location = location;
            ImportedQuantity = importedQuantity;
        }
    }
}

using SLAP.AggregateModels.MaterialAggregate;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class InventoryReceiptEntry 
    {
        public string inventoryReceiptEntryId { get; set; }
        public string lotNumber { get; set; }
        public string purchaseOrderNumber { get; set; }
        public Material material { get; set; }
        public ReceiptLot receiptLot { get; set; }
        public string note { get; set; }

        public InventoryReceiptEntry()
        {
        }

        public InventoryReceiptEntry(string inventoryReceiptEntryId, string purchaseOrderNumber, Material material, string note, string lotNumber)
        {
            this.inventoryReceiptEntryId = inventoryReceiptEntryId;
            this.purchaseOrderNumber = purchaseOrderNumber;
            this.material = material;
            this.note = note;
            this.lotNumber = lotNumber;
            this.receiptLot = new ReceiptLot();
        }
    }
}

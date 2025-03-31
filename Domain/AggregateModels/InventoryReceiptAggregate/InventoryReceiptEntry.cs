using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;

namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class InventoryReceiptEntry
    {
        [Key]
        public string inventoryReceiptEntryId { get; set; }

        public string purchaseOrderNumber { get; set; }

        [ForeignKey("materialId")]
        public string materialId { get; set; }
        public Material material { get; set; }

        public string note { get; set; }

        [ForeignKey("lotNumber")]
        public string lotNumber { get; set; }
        public ReceiptLot receiptLot { get; set; }

        public string InventoryReceiptId { get; set; }
        public InventoryReceipt inventoryReceipt { get; set; }

        public InventoryReceiptEntry()
        {
        }

        public void AddReceiptLot(ReceiptLot receiptLot)
        {
            this.receiptLot = receiptLot;
        }
    }
}

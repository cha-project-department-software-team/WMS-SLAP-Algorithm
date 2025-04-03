namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class InventoryReceiptEntry : Entity, IAggregateRoot
    {
        [Key]
        public string inventoryReceiptEntryId { get; set; }

        public string purchaseOrderNumber { get; set; }
        public string materialId { get; set; }
        public string materialName { get; set; }
        public string note { get; set; }
        public double importedQuantity { get; set; }

        [ForeignKey("lotNumber")]
        public string lotNumber { get; set; }
        public ReceiptLot receiptLot { get; set; }

        public string InventoryReceiptId { get; set; }
        public InventoryReceipt inventoryReceipt { get; set; }

        public InventoryReceiptEntry()
        {
        }

        public InventoryReceiptEntry(string inventoryReceiptEntryId, string purchaseOrderNumber, string materialId, string materialName, string note, double importedQuantity, string lotNumber, string inventoryReceiptId)
        {
            this.inventoryReceiptEntryId = inventoryReceiptEntryId;
            this.purchaseOrderNumber = purchaseOrderNumber;
            this.materialId = materialId;
            this.materialName = materialName;
            this.note = note;
            this.importedQuantity = importedQuantity;
            this.lotNumber = lotNumber;
            InventoryReceiptId = inventoryReceiptId;
        }
    }
}

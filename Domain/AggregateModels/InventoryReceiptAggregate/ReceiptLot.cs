namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptLot : Entity, IAggregateRoot
    {
        [Key]
        public string receiptLotId { get; set; }

        public double importedQuantity { get; set; }
        public List<ReceiptSublot> receiptSublots { get; set; }
        public Material material { get; set; }
        public LotStatus receiptLotStatus { get; set; }

        [ForeignKey("inventoryReceiptEntryId")]
        public string InventoryReceiptEntryId { get; set; }
        public InventoryReceiptEntry inventoryReceiptEntry { get; set; }

        public ReceiptLot()
        {
        }

        public ReceiptLot(string receiptLotId, double importedQuantity, LotStatus receiptLotStatus, string inventoryReceiptEntryId)
        {
            this.receiptLotId = receiptLotId;
            this.importedQuantity = importedQuantity;
            this.receiptSublots = new List<ReceiptSublot>();
            this.receiptLotStatus = receiptLotStatus;
            InventoryReceiptEntryId = inventoryReceiptEntryId;
        }
    }
}

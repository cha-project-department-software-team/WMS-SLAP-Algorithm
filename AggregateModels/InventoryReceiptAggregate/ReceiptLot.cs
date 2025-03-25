using SLAP.Enum;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptLot 
    {
        public string receiptLotId { get; set; }
        public double importedQuantity { get; set; }
        public List<ReceiptSublot> receiptSublots { get; set; }
        public InventoryReceiptEntry inventoryReceiptEntry { get; set; }
        public LotStatus receiptLotStatus { get; set; }

        public ReceiptLot()
        {
        }

        public ReceiptLot(string receiptLotId, double importedQuantity, List<ReceiptSublot> receiptSublots, InventoryReceiptEntry inventoryReceiptEntry, LotStatus receiptLotStatus)
        {
            this.receiptLotId = receiptLotId;
            this.importedQuantity = importedQuantity;
            this.receiptSublots = receiptSublots;
            this.inventoryReceiptEntry = inventoryReceiptEntry;
            this.receiptLotStatus = receiptLotStatus;
        }
    }
}

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class InventoryReceiptEntry
    {
        public string InventoryReceiptEntryId { get; private set; }
        public string PurchaseOrderNumber { get; private set; }
        public string MaterialName { get; private set; }
        public string MaterialCode { get; private set; }
        public double ImportedQuantity { get; private set; }
        public string Note { get; private set; }
        public string InventoryReceiptId { get; private set; }
        public string LotNumber { get; private set; }
        public ReceiptLot ReceiptLot { get; private set; }

        public InventoryReceiptEntry()
        {
        }

        public InventoryReceiptEntry(string inventoryReceiptEntryId, string purchaseOrderNumber, string materialCode, string materialName, string note, string inventoryReceiptId, string lotNumber)
        {
            InventoryReceiptEntryId = inventoryReceiptEntryId;
            PurchaseOrderNumber = purchaseOrderNumber;
            MaterialCode = materialCode;
            MaterialName = materialName;
            Note = note;
            InventoryReceiptId = inventoryReceiptId;
            LotNumber = lotNumber;
        }

        public void AddReceiptLot(ReceiptLot receiptLot)
        {
            ReceiptLot = receiptLot;
        }
    }
}

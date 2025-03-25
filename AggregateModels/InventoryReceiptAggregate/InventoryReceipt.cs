using SLAP.AggregateModels.PartyAggregate;
using SLAP.AggregateModels.StorageAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class InventoryReceipt 
    {
        public string inventoryReceiptId { get; set; }
        public Supplier supplier { get; set; }
        public Person receivedBy { get; set; }
        public Warehouse warehouse { get; set; }
        public DateTime receiptDate { get; set; }
        public ReceiptStatus receiptStatus { get; set; }
        public List<InventoryReceiptEntry> entries { get; set; }
        public InventoryReceipt()
        {
        }

        public InventoryReceipt(string inventoryReceiptId, DateTime receiptDate, ReceiptStatus receiptStatus, Supplier supplier, Person receivedBy, Warehouse warehouse, List<InventoryReceiptEntry> entries)
        {
            this.inventoryReceiptId = inventoryReceiptId;
            this.receiptDate = receiptDate;
            this.receiptStatus = receiptStatus;
            this.supplier = supplier;
            this.receivedBy = receivedBy;
            this.warehouse = warehouse;
            this.entries = entries;
        }
    }
}

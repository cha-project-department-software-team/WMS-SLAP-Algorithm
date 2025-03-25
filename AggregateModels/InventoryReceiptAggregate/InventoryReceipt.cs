using SLAP.AggregateModels.PartyAggregate;
using SLAP.AggregateModels.StorageAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class InventoryReceipt 
    {
        public string inventoryReceiptId { get; set; }
        public DateTime receiptDate { get; set; }
        public ReceiptStatus receiptStatus { get; set; }
        public string supplierId { get; set; }
        public Supplier supplier { get; set; }
        public string personId { get; set; }
        public Person receivedBy { get; set; }
        public string warehouseId { get; set; }
        public Warehouse warehouse { get; set; }

        public List<InventoryReceiptEntry> entries { get; set; }

        public InventoryReceipt(string inventoryReceiptId, DateTime receiptDate, ReceiptStatus receiptStatus, string supplierId, string personId, string warehouseId)
        {
            this.inventoryReceiptId = inventoryReceiptId;
            this.receiptDate = receiptDate;
            this.receiptStatus = receiptStatus;
            this.supplierId = supplierId;
            this.personId = personId;
            this.warehouseId = warehouseId;
            this.entries = new List<InventoryReceiptEntry>();
        }
    }
}

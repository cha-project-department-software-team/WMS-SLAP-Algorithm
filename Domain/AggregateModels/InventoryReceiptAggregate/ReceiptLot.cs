using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.Enum;
using System.Text.Json.Serialization;

namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptLot
    {
        [Key]
        public string receiptLotId { get; set; }

        public double importedQuantity { get; set; }
        public List<ReceiptSublot> receiptSublots { get; set; }
        public LotStatus receiptLotStatus { get; set; }

        [ForeignKey("inventoryReceiptEntryId")]
        public string InventoryReceiptEntryId { get; set; }
        public InventoryReceiptEntry inventoryReceiptEntry { get; set; }

        public ReceiptLot(string receiptLotId, double importedQuantity, LotStatus receiptLotStatus, string inventoryReceiptEntryId)
        {
            this.receiptLotId = receiptLotId;
            this.importedQuantity = importedQuantity;
            this.receiptLotStatus = receiptLotStatus;
            InventoryReceiptEntryId = inventoryReceiptEntryId;
        }
    }
}

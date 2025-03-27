using SLAP.AggregateModels.MaterialAggregate;
using SLAP.Enum;
using System.Text.Json.Serialization;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptLot
    {
        public string ReceiptLotId { get; set; }
        public Material Material { get; set; }
        public double ImportedQuantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LotStatus ReceiptLotStatus { get; set; }
        public List<ReceiptSubLot> ReceiptSublots { get; set; }

        public ReceiptLot()
        {
        }

        public ReceiptLot(string receiptLotId, Material material, double importedQuantity, LotStatus receiptLotStatus)
        {
            this.ReceiptLotId = receiptLotId;
            this.Material = material;
            this.ImportedQuantity = importedQuantity;
            this.ReceiptLotStatus = receiptLotStatus;
            this.ReceiptSublots = new List<ReceiptSubLot>();
        }
    }
}

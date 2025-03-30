using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.Enum;
using System.Text.Json.Serialization;

namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptLot
    {
        public string ReceiptLotId { get; set; }
        public Material Material { get; set; }
        public double ImportedQuantity { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LotStatus ReceiptLotStatus { get; set; }
        public List<ReceiptSublot> ReceiptSublots { get; set; }

        public ReceiptLot()
        {
        }

        public ReceiptLot(string receiptLotId, Material material, double importedQuantity, LotStatus receiptLotStatus)
        {
            ReceiptLotId = receiptLotId;
            Material = material;
            ImportedQuantity = importedQuantity;
            ReceiptLotStatus = receiptLotStatus;
            ReceiptSublots = new List<ReceiptSublot>();
        }
    }
}

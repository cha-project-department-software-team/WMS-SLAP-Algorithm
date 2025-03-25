using SLAP.AggregateModels.StorageAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptSublot
    {
        public string receiptSublotId { get; set; }
        public double importedQuantity { get; set; }
        public LotStatus subLotStatus { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }
        public string locationId { get; set; }
        public Location location { get; set; }
        public string receiptLotId { get; set; }
        public ReceiptLot receiptLot { get; set; }

        public  ReceiptSublot(string receiptSublotId, double importedQuantity, LotStatus subLotStatus, UnitOfMeasure unitOfMeasure, string locationId, string receiptLotId)
        {
            this.receiptSublotId = receiptSublotId;
            this.importedQuantity = importedQuantity;
            this.subLotStatus = subLotStatus;
            this.unitOfMeasure = unitOfMeasure;
            this.locationId = locationId;
            this.receiptLotId = receiptLotId;
        }
    }
}

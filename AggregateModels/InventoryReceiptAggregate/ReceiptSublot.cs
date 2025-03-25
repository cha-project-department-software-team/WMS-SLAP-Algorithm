using SLAP.AggregateModels.StorageAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptSublot
    {
        public string receiptSublotId { get; set; }
        public double importedQuantity { get; set; }
        public Location location { get; set; }
        public ReceiptLot receiptLot { get; set; }
        public LotStatus subLotStatus { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        public ReceiptSublot()
        {
        }

        public ReceiptSublot(string receiptSublotId, double importedQuantity, LotStatus subLotStatus, UnitOfMeasure unitOfMeasure, Location location, ReceiptLot receiptLot)
        {
            this.receiptSublotId = receiptSublotId;
            this.importedQuantity = importedQuantity;
            this.subLotStatus = subLotStatus;
            this.unitOfMeasure = unitOfMeasure;
            this.location = location;
            this.receiptLot = receiptLot;
        }
    }
}

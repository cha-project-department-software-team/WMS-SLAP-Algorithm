namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptSublot : Entity, IAggregateRoot
    {
        [Key]
        public string receiptSublotId { get; set; }

        public double importedQuantity { get; set; }
        public LotStatus subLotStatus { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        [ForeignKey("locationId")]
        public string locationId { get; set; }
        public Location location { get; set; }

        [ForeignKey("receiptLotId")]
        public string receiptLotId { get; set; }
        public ReceiptLot receiptLot { get; set; }

        public ReceiptSublot()
        {
        }

        public ReceiptSublot(string receiptSublotId, double importedQuantity, LotStatus subLotStatus, UnitOfMeasure unitOfMeasure, string locationId, string receiptLotId)
        {
            this.receiptSublotId = receiptSublotId;
            this.importedQuantity = importedQuantity;
            this.subLotStatus = subLotStatus;
            this.unitOfMeasure = unitOfMeasure;
            this.locationId = locationId;
            this.receiptLotId = receiptLotId;
        }

        #region Retrieve Methods    

        public Material? GetMaterial()
        {
            if (this.receiptLot is not null)
            {
                return this.receiptLot.material;
            }

            return null;
        }

        #endregion

        #region Update Location

        /// <summary>
        /// Update the Locations of the Receipt Sublot.
        /// </summary>
        /// <param name="location"></param>
        public void UpdateLocation(Location location)
        {
            if (location is not null)
            {
                this.locationId = location.locationId;
                this.location = location;
            }
        }

        /// <summary>
        /// Update the object for Receipt Lot
        /// </summary>
        /// <param name="receiptLot"></param>
        public void UpdateReceiptLot(ReceiptLot receiptLot)
        {
            if (receiptLot is not null)
            {
                this.receiptLot = receiptLot;
            }
        }

        #endregion
    }
}

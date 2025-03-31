using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations;

namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptSublot
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

        #region Constructors
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

        //public ReceiptSublot(string receiptSublotId, Material material, Location? location, double importedQuantity)
        //{
        //    this.receiptSublotId = receiptSublotId;
        //    this.material = material;
        //    this.location = location;
        //    this.importedQuantity = importedQuantity;
        //}

        #endregion

        #region Update Location

        /// <summary>
        /// Update the Locations of the Receipt Sublot.
        /// </summary>
        /// <param name="location"></param>
        public void UpdateLocation(Location location)
        {
            if (location.IsValid())
            {
                this.location = location;
            }
        }

        #endregion


        #region Validation Method
        //private bool? _isValid { get; set; }

        ///// <summary>
        ///// Validate the Receipt Sublot object
        ///// </summary>
        ///// <returns></returns>
        //public bool IsValid()
        //{
        //    if (_isValid.HasValue)
        //        return _isValid.Value;

        //    _isValid = !string.IsNullOrEmpty(this.receiptSublotId) && this.material.IsValid() && Location != null && ImportedQuantity > 0;
        //    return _isValid.Value;
        //}

        #endregion
    }
}

using SLAP.AggregateModels.MaterialAggregate;
using SLAP.AggregateModels.StorageAggregate;

namespace SLAP.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptSubLot
    {
        public string ReceiptSublotId { get; private set; }
        public Material Material { get; private set; }
        public Location? Location { get; private set; }
        public double ImportedQuantity { get; private set; }

        #region Constructors
        public ReceiptSubLot()
        {
            this.ReceiptSublotId = string.Empty;
            this.Material = new Material();
            this.Location = null;
            this.ImportedQuantity = 0.0;
        }

        public ReceiptSubLot(string receiptSublotId, Material material, Location? location, double importedQuantity)
        {
            ReceiptSublotId = receiptSublotId;
            Material = material;
            Location = location;
            ImportedQuantity = importedQuantity;
        }

        #endregion

        #region Update Location

        /// <summary>
        /// Update the Location of the Receipt Sublot.
        /// </summary>
        /// <param name="location"></param>
        public void UpdateLocation(Location location)
        {
            if (location.IsValid())
            {
                this.Location = location;
            }
        }

        #endregion


        #region Validation Method
        private bool? _isValid { get; set; }

        /// <summary>
        /// Validate the Receipt Sublot object
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (_isValid.HasValue)
                return _isValid.Value;

            _isValid = !string.IsNullOrEmpty(this.ReceiptSublotId) && this.Material.IsValid() && this.Location != null && this.ImportedQuantity > 0;
            return _isValid.Value;
        }

        #endregion
    }
}

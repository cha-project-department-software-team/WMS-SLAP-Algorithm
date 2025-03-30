using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations;

namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class ReceiptSublot
    {
        public string ReceiptSublotId { get; private set; }
        public Material Material { get; private set; }
        public Location? Location { get; private set; }
        public double ImportedQuantity { get; private set; }

        #region Constructors
        public ReceiptSublot()
        {

        }

        public ReceiptSublot(string receiptSublotId, Material material, Location? location, double importedQuantity)
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
                Location = location;
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

            _isValid = !string.IsNullOrEmpty(ReceiptSublotId) && Material.IsValid() && Location != null && ImportedQuantity > 0;
            return _isValid.Value;
        }

        #endregion
    }
}

using SLAP.Enum;
using SLAPScheduling.AggregateModels.Properties;
using SLAPScheduling.Extensions;
using SLAPScheduling.Utilities;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class Material
    {
        public string MaterialId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialClassId { get; set; }
        public List<Property> Properties { get; set; }

        #region Constructors
        public Material()
        {
        }

        public Material(string materialId, string materialName, string materialClassId)
        {
            MaterialId = materialId;
            MaterialName = materialName;
            MaterialClassId = materialClassId;
            Properties = new List<Property>();
        }

        #endregion

        #region Calculate Volume Size

        /// <summary>
        /// Calculate the Volume of Material as Cubic Meter.
        /// </summary>
        /// <returns></returns>
        public double GetVolume()
        {
            double length = this.Properties.GetSizeParameter("Length");
            double width = this.Properties.GetSizeParameter("Width");
            double height = this.Properties.GetSizeParameter("Height");

            return length * width * height;
        }

        #endregion

        #region Retrieve Allow Storage Level 

        /// <summary>
        /// Retrieve the allow storage level from Material Properties
        /// </summary>
        /// <returns></returns>
        public int GetLimitStorageLevel()
        {
            if (this.Properties.TryGetPropertyValue("Storage Level", out string level, out UnitOfMeasure none))
            {
                return int.TryParse(level, out int storageLevel) ? storageLevel : 0;
            }

            return 0;
        }

        #endregion

        #region Calculate the movement ratio of a product

        /// <summary>
        /// Calculate the movement ratio of a product.
        /// </summary>
        /// <returns></returns>
        public double GetMovementRatio()
        {
            var numberOfOrdersInMonth = GetNumberOfOrdersInAMonth();
            var numberOfStorageLocations = GetNumberOfStorageLocations();

            return numberOfOrdersInMonth / numberOfStorageLocations;
        }


        /// <summary>
        /// Tp means the number of issue orders for this product in a month (order/ month).
        /// </summary>
        /// <returns></returns>
        private int GetNumberOfOrdersInAMonth()
        {
            if (this.Properties.TryGetPropertyValue("Tp", out string propertyValue, out UnitOfMeasure unitOfMeasure))
            {
                return int.TryParse(propertyValue, out int tp) ? tp : 0;
            }

            return 0;
        }

        /// <summary>
        /// Sp means the number of storage locations for this product (location).
        /// </summary>
        /// <returns></returns>
        private int GetNumberOfStorageLocations()
        {
            if (this.Properties.TryGetPropertyValue("Sp", out string propertyValue, out UnitOfMeasure unitOfMeasure))
            {
                return int.TryParse(propertyValue, out int sp) ? sp : 0;
            }

            return 0;
        }

        #endregion

        #region Validation Method
        private bool? _isValid { get; set; }

        /// <summary>
        /// Validate the Material object
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (_isValid.HasValue)
                return _isValid.Value;

            _isValid = !string.IsNullOrEmpty(this.MaterialId) && !string.IsNullOrEmpty(this.MaterialName) && !string.IsNullOrEmpty(this.MaterialClassId);
            return _isValid.Value;
        }

        #endregion
    }
}

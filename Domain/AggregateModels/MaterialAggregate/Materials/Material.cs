using SLAPScheduling.Algorithm.Extensions;
using SLAPScheduling.Domain.AggregateModels.InventoryIssueAggregate;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialClasses;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialLots;
using SLAPScheduling.Domain.Enum;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials
{
    public class Material : Entity, IAggregateRoot
    {
        [Key]
        public string materialId { get; set; }
        public string materialName { get; set; }

        [ForeignKey("materialClassId")]
        public string materialClassId { get; set; }
        public MaterialClass materialClass { get; set; }
        public List<MaterialProperty> properties { get; set; }
        public List<MaterialLot> lots { get; set; }
        public List<InventoryReceiptEntry> receiptEntries { get; set; }
        public List<InventoryIssueEntry> issueEntries { get; set; }

        public Material(string materialId, string materialName, string materialClassId)
        {
            this.materialId = materialId;
            this.materialName = materialName;
            this.materialClassId = materialClassId;
            this.properties = new List<MaterialProperty>();
        }

        #region Calculate Volume Size

        /// <summary>
        /// Calculate the Volume of Material as Cubic Meter.
        /// </summary>
        /// <returns></returns>
        public double GetVolume()
        {
            double length = this.properties.GetSizeParameter("Length");
            double width = this.properties.GetSizeParameter("Width");
            double height = this.properties.GetSizeParameter("Height");

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
            if (this.properties.TryGetPropertyValue("Storage Level", out string level, out UnitOfMeasure none))
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
            if (this.properties.TryGetPropertyValue("Tp", out string propertyValue, out UnitOfMeasure unitOfMeasure))
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
            if (this.properties.TryGetPropertyValue("Sp", out string propertyValue, out UnitOfMeasure unitOfMeasure))
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

            _isValid = !string.IsNullOrEmpty(materialId) && !string.IsNullOrEmpty(materialName) && !string.IsNullOrEmpty(materialClassId);
            return _isValid.Value;
        }

        #endregion
    }
}

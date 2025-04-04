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
        }

        #region Calculate Volume Size

        private double? _packetVolumeSize;

        /// <summary>
        /// Calculate the Volume of Material Packet as Cubic Meter.
        /// </summary>
        /// <returns></returns>
        public double GetPacketVolume()
        {
            if (_packetVolumeSize.HasValue)
                return _packetVolumeSize.Value;

            if (this.properties.TryGetPropertyValue("Volume", out string propertyValue, out UnitOfMeasure unit) && double.TryParse(propertyValue, out double volume))
            {
                _packetVolumeSize = unit == UnitOfMeasure.CubicMeters ? volume : 0;
            }
            else
            {
                double length = this.properties.GetSizeParameter("Length");
                double width = this.properties.GetSizeParameter("Width");
                double height = this.properties.GetSizeParameter("Height");

                _packetVolumeSize = length * width * height;
            }

            return _packetVolumeSize ?? 0;
        }

        private int? _packetSize;

        /// <summary>
        /// Retrieve the number of material in each package.
        /// </summary>
        /// <returns></returns>
        public int GetPacketSize()
        {
            if (_packetSize.HasValue)
                return _packetSize.Value;

            if (this.properties.TryGetPropertyValue("PacketSize", out string propertyValue, out UnitOfMeasure unit))
            {
                _packetSize = int.TryParse(propertyValue, out int packetSize) ? packetSize : 0;
            }

            return _packetSize ?? 0;
        }

        #endregion

        #region Retrieve Allow Storage Level 

        private int? _storageLevel;

        /// <summary>
        /// Retrieve the allow storage level from Material Properties
        /// </summary>
        /// <returns></returns>
        public int GetLimitStorageLevel()
        {
            if (_storageLevel.HasValue)
                return _storageLevel.Value;

            if (this.properties.TryGetPropertyValue("StorageLevel", out string level, out UnitOfMeasure none))
            {
                _storageLevel = int.TryParse(level, out int storageLevel) ? storageLevel : 0;
            }

            return _storageLevel ?? 0;
        }

        #endregion

        #region Calculate the movement ratio of a product

        private double? _movementRatio;

        /// <summary>
        /// Calculate the movement ratio of a product.
        /// </summary>
        /// <returns></returns>
        public double GetMovementRatio()
        {
            if (_movementRatio.HasValue)
                return _movementRatio.Value;

            var numberOfOrdersInMonth = GetNumberOfOrdersInAMonth();
            var numberOfStorageLocations = GetNumberOfStorageLocations();
            if (numberOfStorageLocations > 0)
            {
                _movementRatio = numberOfOrdersInMonth / numberOfStorageLocations;
            }
           
            return _movementRatio ?? 0;
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
    }
}

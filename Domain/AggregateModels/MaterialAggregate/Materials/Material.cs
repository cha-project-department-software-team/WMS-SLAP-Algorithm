using SLAPScheduling.Algorithm.Utilities;

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

            if (this.properties.TryGetPropertyValue("VolumePacket", out string propertyValue, out UnitOfMeasure unit) && double.TryParse(propertyValue, out double volume))
            {
                _packetVolumeSize = unit == UnitOfMeasure.CubicMeters ? volume : 0;
            }
            else
            {
                double length = this.GetPacketLength();
                double width = this.GetPacketWidth();
                double height = this.GetPacketHeight();

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

        private double? _packetLength;
        private double? _packetWidth;
        private double? _packetHeight;

        /// <summary>
        /// Retrieve the length of packet volume
        /// </summary>
        /// <returns></returns>
        public double GetPacketLength()
        {
            if (_packetLength.HasValue)
                return _packetLength.Value;

            if (this.properties?.Count > 0 && this.properties.TryGetPropertyValue("Length", out string propertyValue, out UnitOfMeasure unitOfMeasure))
            {
                _packetLength = double.TryParse(propertyValue, out double sizeValue) ? sizeValue * Utility.GetMeterMultiplier(unitOfMeasure) : 0;
            }

            return _packetLength ?? 0.0;
        }

        /// <summary>
        /// Retrieve the width of packet volume
        /// </summary>
        /// <returns></returns>
        public double GetPacketWidth()
        {
            if (_packetWidth.HasValue)
                return _packetWidth.Value;

            if (this.properties?.Count > 0 && this.properties.TryGetPropertyValue("Width", out string propertyValue, out UnitOfMeasure unitOfMeasure))
            {
                _packetWidth = double.TryParse(propertyValue, out double sizeValue) ? sizeValue * Utility.GetMeterMultiplier(unitOfMeasure) : 0;
            }

            return _packetWidth ?? 0.0;
        }

        /// <summary>
        /// Retrieve the height of packet volume
        /// </summary>
        /// <returns></returns>
        public double GetPacketHeight()
        {
            if (_packetHeight.HasValue)
                return _packetHeight.Value;

            if (this.properties?.Count > 0 && this.properties.TryGetPropertyValue("Height", out string propertyValue, out UnitOfMeasure unitOfMeasure))
            {
                _packetHeight = double.TryParse(propertyValue, out double sizeValue) ? sizeValue * Utility.GetMeterMultiplier(unitOfMeasure) : 0;
            }

            return _packetHeight ?? 0.0;
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
        private int? _Tp;
        private int? _Sp;
        
        /// <summary>
        /// Calculate the movement ratio of a product.
        /// </summary>
        /// <returns></returns>
        public double GetMovementRatio()
        {
            if (_movementRatio.HasValue)
                return _movementRatio.Value;

            int Tp = GetNumberOfPickingOrdersInMonth();
            int Sp = GetNumberOfStorageLocations();
            if (Sp > 0)
            {
                _movementRatio = (double)Tp / (double)Sp;
            }

            return _movementRatio ?? 0.0;
        }

        /// <summary>
        /// Tp means the number of issue orders for this product in a month (orders/ month).
        /// </summary>
        /// <returns></returns>
        private int GetNumberOfPickingOrdersInMonth()
        {
            if (_Tp.HasValue)
                return _Tp.Value;

            if (this.properties.TryGetPropertyValue("Tp", out string propertyValue, out UnitOfMeasure unitOfMeasure))
            {
                _Tp = int.TryParse(propertyValue, out int tp) ? tp : 0;
            }

            return _Tp ?? 0;
        }

        /// <summary>
        /// Tp means the number of issue orders for this product in a month (order/ month).
        /// </summary>
        /// <returns></returns>
        private int GetNumberOfStorageLocations()
        {
            return this._Sp ?? 0;
        }

        /// <summary>
        /// Sp means the number of storage locations for this product (location).
        /// </summary>
        /// <returns></returns>
        public void UpdateNumberOfStorageLocations(int numberOfStorageLocations)
        {
            this._Sp = numberOfStorageLocations;
        }

        #endregion
    }
}

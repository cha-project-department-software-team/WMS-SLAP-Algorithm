using SLAPScheduling.Algorithm.Extensions;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialSubLots;
using SLAPScheduling.Domain.AggregateModels.Properties;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations
{
    public class Location : Entity, IAggregateRoot
    {
        [Key]
        public string locationId { get; set; }
        public List<MaterialSubLot> materialSubLots { get; set; }
        public List<ReceiptSublot> receiptSublots { get; set; }
        public List<LocationProperty> properties { get; set; }

        [ForeignKey("warehouseId")]
        public string warehouseId { get; set; }
        public Warehouse warehouse { get; set; }
        private int? _rackIndex { get; set; }
        private int? _rowIndex { get; set; }
        private int? _columnIndex { get; set; }
        private int? _levelIndex { get; set; }

        #region Constructors
        public Location(string locationId, string warehouseId)
        {
            this.locationId = locationId;
            this.warehouseId = warehouseId;
        }

        public Location(string locationId)
        {
            if (!string.IsNullOrEmpty(locationId) && TryGetLocationIdentification(out string warehouseId, out int rackIndex, out int rowIndex, out int columnIndex, out int levelIndex))
            {
                this.locationId = locationId;
                this.warehouseId = warehouseId;
                _rackIndex = rackIndex;
                _rowIndex = rowIndex;
                _columnIndex = columnIndex;
                _levelIndex = levelIndex;
            }
        }

        #endregion

        #region Retrieve Methods

        public int GetLevelIndex()
        {
            return _levelIndex ?? 0;
        }

        #endregion

        #region Calculate Volume Size

        private static double? _locationVolume { get; set; }

        /// <summary>
        /// Calculate the Volume of Material as Cubic Meter.
        /// </summary>
        /// <returns></returns>
        public double GetVolume()
        {
            if (_locationVolume.HasValue)
                return _locationVolume.Value;

            double length = this.properties.GetSizeParameter("Length");
            double width = this.properties.GetSizeParameter("Width");
            double height = this.properties.GetSizeParameter("Height");

            _locationVolume = length * width * height;
            return _locationVolume.Value;
        }

        #endregion

        #region Calculate Filled Storage Volume

        /// <summary>
        /// Calculate the percentage of volume used for storing material sublots.
        /// </summary>
        /// <returns></returns>
        public double GetCurrentStoragePercentage()
        {
            var locationVolume = GetVolume();
            if (locationVolume == 0)
                return 0;

            var storageSubLotVolume = GetCurrentSubLotStorageVolume();
            return storageSubLotVolume < locationVolume ? storageSubLotVolume / locationVolume : 1;
        }

        /// <summary>
        /// Calculate the total volume which is filled by material sublots.
        /// </summary>
        /// <returns></returns>
        private double GetCurrentSubLotStorageVolume()
        {
            if (this.materialSubLots?.Count > 0)
            {
                return this.materialSubLots.Sum(sublot =>
                {
                    var material = sublot.GetMaterial();
                    return material != null ? material.GetVolume() * sublot.existingQuantity : 0;
                });
            }

            return 0;
        }

        /// <summary>
        /// Calculate the storage percentage of receipt sublot in the location.
        /// </summary>
        /// <param name="receiptSubLot"></param>
        /// <returns></returns>
        public double GetStoragePercentage(ReceiptSublot receiptSubLot)
        {
            var subLotVolume = receiptSubLot.IsValid() ? receiptSubLot.Material.GetVolume() * receiptSubLot.ImportedQuantity : 0;

            var locationVolume = GetVolume();
            return locationVolume > 0 ? subLotVolume / locationVolume : 0;
        }

        #endregion

        #region Calculate the Distance to I/O point

        private static double? _distanceToIOPoint { get; set; }

        /// <summary>
        /// Calculate the distance between the location and the I/O point.
        /// </summary>
        /// <returns></returns>
        public double GetDistanceToIOPoint()
        {
            if (_distanceToIOPoint.HasValue)
                return _distanceToIOPoint.Value;

            if (TryGetLocationIdentification(out string warehouseId, out int rackIndex, out int rowIndex, out int columnIndex, out int levelIndex))
            {
                var aisleWidth = this.warehouse.GetAisleWidth();
                var rackWidth = this.warehouse.GetRackWidth();

                var locationLength = this.properties.GetSizeParameter("Length");
                var locationHeight = this.properties.GetSizeParameter("Height");

                var xDistance = (columnIndex - 0.5) * locationLength;
                var yDistance = 0.5 * rowIndex * aisleWidth + (rowIndex - 1) * (rackWidth + 0.5 * aisleWidth) + (rackWidth + aisleWidth) * (rackIndex - 1);
                var zDistance = locationHeight * (levelIndex - 1);

                _distanceToIOPoint = xDistance + yDistance + zDistance;
                return _distanceToIOPoint.Value;
            }

            return 0;
        }

        public bool TryGetLocationIdentification(out string warehouseId, out int rackIndex, out int rowIndex, out int columnIndex, out int levelIndex)
        {
            var locationParts = this.locationId.Split('.');
            if (locationParts?.Length == 5)
            {
                if (int.TryParse(locationParts[1], out rackIndex) &&
                    int.TryParse(locationParts[2], out rowIndex) &&
                    int.TryParse(locationParts[3], out columnIndex) &&
                    int.TryParse(locationParts[4], out levelIndex))
                {
                    warehouseId = locationParts[0];
                    return true;
                }
            }

            (warehouseId, rackIndex, rowIndex, columnIndex, levelIndex) = (string.Empty, 0, 0, 0, 0);
            return false;
        }

        #endregion

        #region Validation Method

        private static bool? _isValid { get; set; }

        /// <summary>
        /// Check the validation of the Location object
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (_isValid.HasValue)
                return _isValid.Value;

            _isValid = false;
            if (!string.IsNullOrEmpty(this.locationId) && this.warehouse != null)
            {
                if (_rackIndex.HasValue && _rowIndex.HasValue && _columnIndex.HasValue && _levelIndex.HasValue)
                {
                    _isValid = true;
                }
                else if (TryGetLocationIdentification(out string warehouseId, out int rackIndex, out int rowIndex, out int columnIndex, out int levelIndex))
                {
                    _rackIndex = rackIndex; _rowIndex = rowIndex; _columnIndex = columnIndex; _levelIndex = levelIndex;
                    _isValid = this.warehouse.warehouseId == warehouseId;
                }
            }

            return _isValid.Value;
        }

        #endregion
    }
}

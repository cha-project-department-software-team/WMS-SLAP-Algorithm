using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using SLAPScheduling.AggregateModels.Properties;
using SLAPScheduling.Extensions;

namespace SLAP.AggregateModels.StorageAggregate
{
    public class Location
    {
        public string LocationId { get; set; }
        public string WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public List<Property> Properties { get; set; }
        public List<MaterialSubLot> MaterialSubLots { get; set; }
        private int? _rackIndex { get; set; }
        private int? _rowIndex { get; set; }
        private int? _columnIndex { get; set; }
        private int? _levelIndex { get; set; }

        #region Constructors
        public Location()
        {
        }

        public Location(string locationId)
        {
            if (!string.IsNullOrEmpty(locationId) && TryGetLocationIdentification(out string warehouseId, out int rackIndex, out int rowIndex, out int columnIndex, out int levelIndex))
            {
                this.LocationId = locationId;
                this.WarehouseId = warehouseId;
                this._rackIndex = rackIndex;
                this._rowIndex = rowIndex;
                this._columnIndex = columnIndex;
                this._levelIndex = levelIndex;
            }
        }

        #endregion

        #region Retrieve Methods

        public int GetLevelIndex()
        {
            return this._levelIndex ?? 0;
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

            double length = this.Properties.GetSizeParameter("Length");
            double width = this.Properties.GetSizeParameter("Width");
            double height = this.Properties.GetSizeParameter("Height");

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
            if (this.MaterialSubLots?.Count > 0)
            {
                return this.MaterialSubLots.Sum(sublot => {
                    var material = sublot.GetMaterial();
                    return material != null ? material.GetVolume() * sublot.ExistingQuality : 0;
                });
            }

            return 0;
        }

        /// <summary>
        /// Calculate the storage percentage of receipt sublot in the location.
        /// </summary>
        /// <param name="receiptSubLot"></param>
        /// <returns></returns>
        public double GetStoragePercentage(ReceiptSubLot receiptSubLot)
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
                var aisleWidth = this.Warehouse.GetAisleWidth();
                var rackWidth = this.Warehouse.GetRackWidth();

                var locationLength = this.Properties.GetSizeParameter("Length");
                var locationHeight = this.Properties.GetSizeParameter("Height");

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
            var locationParts = this.LocationId.Split('.');
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
            if (!string.IsNullOrEmpty(this.LocationId) && this.Warehouse != null)
            {
                if (this._rackIndex.HasValue && this._rowIndex.HasValue && this._columnIndex.HasValue && this._levelIndex.HasValue)
                {
                    _isValid = true;
                }
                else if (TryGetLocationIdentification(out string warehouseId, out int rackIndex, out int rowIndex, out int columnIndex, out int levelIndex))
                {
                    this._rackIndex = rackIndex; this._rowIndex = rowIndex; this._columnIndex = columnIndex; this._levelIndex = levelIndex;
                    _isValid = this.Warehouse.WarehouseId == warehouseId;
                }
            }

            return _isValid.Value;
        }

        #endregion
    }
}

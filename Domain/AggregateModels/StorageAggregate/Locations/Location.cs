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

        public Location(string locationId, string warehouseId)
        {
            this.locationId = locationId;
            this.warehouseId = warehouseId;
        }

        #region Retrieve Methods

        public int GetRowIndex()
        {
            if (_rowIndex is not null)
                return _rowIndex.Value;

            if (!string.IsNullOrEmpty(locationId) && TryGetLocationIdentification(out string warehouseId, out int rackIndex, out int rowIndex, out int columnIndex, out int levelIndex))
            {
                _rackIndex = rackIndex;
                _rowIndex = rowIndex;
                _columnIndex = columnIndex;
                _levelIndex = levelIndex;
            }

            return _rowIndex ?? 0;
        }

        public int GetStorageLevel()
        {
            if (_levelIndex is not null)
                return _levelIndex.Value;

            if (!string.IsNullOrEmpty(locationId) && TryGetLocationIdentification(out string warehouseId, out int rackIndex, out int rowIndex, out int columnIndex, out int levelIndex))
            {
                _rackIndex = rackIndex;
                _rowIndex = rowIndex;
                _columnIndex = columnIndex;
                _levelIndex = levelIndex;
            }

            return _levelIndex ?? 0;
        }

        #endregion

        #region Receipt Sublots

        public List<ReceiptSublot>? GetReceiptSublots()
        {
            return this.receiptSublots?.Count > 0 ? this.receiptSublots : null;
        }

        public void AddReceiptSublot(ReceiptSublot receiptSublot)
        {
            if (this.receiptSublots is null)
                this.receiptSublots = new List<ReceiptSublot>();

            this.receiptSublots.Add(receiptSublot);
        }

        public bool RemoveReceiptSubLot(ReceiptSublot receiptSublot)
        {
            if (this.receiptSublots?.Count > 0)
            {
                var sublotIndex = this.receiptSublots.FindIndex(x => x.Equals(receiptSublot));
                if (sublotIndex != -1)
                {
                    this.receiptSublots.RemoveAt(sublotIndex);
                    return true;
                }
            }

            return false;
        }

        public double GetReceiptAndMaterialStoragePercentage()
        {
            var storingPercent = this.GetCurrentStoragePercentage();
            if (this.receiptSublots?.Count > 0)
            {
                var assignedPercent = this.receiptSublots.Sum(sublot => sublot.GetStoragePercentage(this));    
                return storingPercent + assignedPercent;
            }

            return storingPercent;
        }

        #endregion

        #region Calculate Volume Size

        private double? _locationVolume { get; set; }

        /// <summary>
        /// Calculate the Volume of Material as Cubic Meter.
        /// </summary>
        /// <returns></returns>
        public double GetLocationVolume()
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
            var locationVolume = GetLocationVolume();
            if (locationVolume == 0)
                return 1.0;

            var storageSubLotVolume = GetCurrentSubLotStorageVolume();
            return storageSubLotVolume < locationVolume ? storageSubLotVolume / locationVolume : 1.0;
        }

        /// <summary>
        /// Calculate the total volume which is filled by material sublots.
        /// </summary>
        /// <returns></returns>
        private double GetCurrentSubLotStorageVolume()
        {
            if (this.materialSubLots?.Count > 0)
            {
                return this.materialSubLots.Sum(sublot => sublot.GetSublotVolume());
            }

            return 0;
        }

        #endregion

        #region Calculate the Distance to I/O point

        private double? _distanceToIOPoint { get; set; }

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

            warehouseId = string.Empty;
            rackIndex = rowIndex = columnIndex = levelIndex = 0;

            // Kiểm tra xem locationId có bị null không
            if (string.IsNullOrWhiteSpace(this.locationId))
            {
                return false;
            }

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
            else if (locationParts?.Length == 4)
            {
                if (int.TryParse(locationParts[1], out rackIndex) &&
                    int.TryParse(locationParts[2], out columnIndex) &&
                    int.TryParse(locationParts[3], out levelIndex))
                {
                    rowIndex = 1;
                    warehouseId = locationParts[0];
                    return true;
                }
            }

            (warehouseId, rackIndex, rowIndex, columnIndex, levelIndex) = (string.Empty, 0, 0, 0, 0);
            return false;
        }

        #endregion
    }
}

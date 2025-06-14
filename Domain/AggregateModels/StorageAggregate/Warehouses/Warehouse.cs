﻿namespace SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses
{
    public class Warehouse : Entity, IAggregateRoot
    {
        [Key]
        public string warehouseId { get; set; }

        public string warehouseName { get; set; }

        public List<Location> locations { get; set; }
        public List<InventoryReceipt> inventoryReceipts { get; set; }
        public List<InventoryIssue> inventoryIssues { get; set; }
        public List<InventoryLog> inventoryLogs { get; set; }
        public List<MaterialLotAdjustment> materialLotAdjustments { get; set; }
        public List<WarehouseProperty> properties { get; set; }

        public Warehouse(string warehouseId, string warehouseName)
        {
            this.warehouseId = warehouseId;
            this.warehouseName = warehouseName;
        }

        #region Retrieve Location Volume from Warehouse

        /// <summary>
        /// Retrieve the volume size of location with the assumption that all locations have the same size.
        /// </summary>
        /// <returns></returns>
        public double GetLocationVolume()
        {
            if (this.locations?.Count > 0)
            {
                var location = this.locations.FirstOrDefault(x => x.locationId != "Empty");
                return location is not null ? location.GetLocationVolume() : 0;
            }

            return 0;
        }

        #endregion

        #region Retrieve Size Parameters from Warehouse

        /// <summary>
        /// Retrieve the width of the aisle in the warehouse.
        /// </summary>
        /// <returns></returns>
        public double GetAisleWidth()
        {
            return this.properties?.Count > 0 ? this.properties.GetSizeParameter("AisleWidth") : 0;
        }

        /// <summary>
        /// Retrieve the width of the rack in the warehouse.
        /// </summary>
        /// <returns></returns>
        public double GetRackWidth()
        {
            return this.properties?.Count > 0 ? this.properties.GetSizeParameter("RackWidth") : 0;
        }

        /// <summary>
        /// Retrieve the distance from Origin point to I/O point in X-axis
        /// </summary>
        /// <returns></returns>
        public double GetXDistance()
        {
            return this.properties?.Count > 0 ? this.properties.GetSizeParameter("Dx") : 0;
        }

        /// <summary>
        /// Retrieve the distance from Origin point to I/O point in Y-axis
        /// </summary>
        /// <returns></returns>
        public double GetYDistance()
        {
            return this.properties?.Count > 0 ? this.properties.GetSizeParameter("Dy") : 0;
        }

        #endregion
    }
}

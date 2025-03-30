using SLAPScheduling.Algorithm.Extensions;
using SLAPScheduling.Domain.AggregateModels.InventoryIssueAggregate;
using SLAPScheduling.Domain.AggregateModels.InventoryLogAggregate;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialLotAdjustmentAggregate;
using SLAPScheduling.Domain.AggregateModels.Properties;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations;

namespace SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses
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
                var location = this.locations.FirstOrDefault();
                return location != null ? location.GetVolume() : 0;
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

        #endregion
    }
}

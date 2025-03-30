using SLAPScheduling.AggregateModels.Properties;
using SLAPScheduling.Extensions;

namespace SLAP.AggregateModels.StorageAggregate
{
    public class Warehouse
    {
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public List<Property> Properties { get; set; }
        public List<Location> Locations { get; set; }

        public Warehouse()
        {
        }

        public Warehouse(string warehouseId, string warehouseName, List<Location> locations)
        {
            this.WarehouseId = warehouseId;
            this.WarehouseName = warehouseName;
            this.Locations = locations;
        }

        #region Retrieve Location Volume from Warehouse

        /// <summary>
        /// Retrieve the volume size of location with the assumption that all locations have the same size.
        /// </summary>
        /// <returns></returns>
        public double GetLocationVolume()
        {
            if (this.Locations?.Count > 0)
            {
                var location = this.Locations.FirstOrDefault();
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
            return this.Properties?.Count > 0 ? this.Properties.GetSizeParameter("AisleWidth") : 0;
        }

        /// <summary>
        /// Retrieve the width of the rack in the warehouse.
        /// </summary>
        /// <returns></returns>
        public double GetRackWidth()
        {
            return this.Properties?.Count > 0 ? this.Properties.GetSizeParameter("RackWidth") : 0;
        }

        #endregion
    }
}

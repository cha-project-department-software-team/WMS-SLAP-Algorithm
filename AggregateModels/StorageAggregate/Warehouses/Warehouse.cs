namespace SLAP.AggregateModels.StorageAggregate
{
    public class Warehouse
    {
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public List<Location> Locations { get; set; }

        public Warehouse()
        {
        }

        public Warehouse(string warehouseId, string warehouseName, List<Location> locations)
        {
            WarehouseId = warehouseId;
            WarehouseName = warehouseName;
            Locations = locations;
        }


        /// <summary>
        /// Retrieve the volume size of location with the assumption that all locations have the same size.
        /// </summary>
        /// <returns></returns>
        public double GetLocationVolumeSize()
        {
            if (Locations?.Count > 0)
            {
                var location = Locations.FirstOrDefault();
                return location != null ? location.GetVolumeSize() : 0;
            }

            return 0;
        }
    }
}

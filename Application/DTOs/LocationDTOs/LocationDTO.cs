namespace SLAPScheduling.Application.DTOs.LocationDTOs
{
    public class LocationDTO
    {
        public string LocationId { get; set; }
        public string WarehouseId { get; set; }
        public List<LocationPropertyDTO> LocationPropertyDTOs { get; set; }

        public LocationDTO()
        {
        }

        public LocationDTO(string locationId, string warehouseId)
        {
            LocationId = locationId;
            WarehouseId = warehouseId;
            LocationPropertyDTOs = new List<LocationPropertyDTO>();
        }

    }
}

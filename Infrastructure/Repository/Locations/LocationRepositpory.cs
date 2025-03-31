namespace SLAPScheduling.Infrastructure.Repository.Locations
{
    public class LocationRepositpory : BaseRepository, ILocationRepository
    {
        public LocationRepositpory(SLAPDbContext context) : base(context)
        {
        }

        public async Task<List<Location>> GetAllLocations()
        {
            var locations = await _context.Locations
                .Include(s => s.properties)
                .AsNoTracking()
                .ToListAsync();

            //var locations = await _context.Locations.ToListAsync();

            //var locationList = new List<Location>();

            ////foreach (var location in locations)
            ////{
            ////    string LocationId = "";

            ////    foreach (var property in location.properties)
            ////    {
            ////        LocationId = property.locationId;
            ////    }

            ////    var newLocation = new Location(LocationId, location.warehouseId, location.properties);

            ////    locationList.Add(newLocation);
            ////}

            return locations;

        }


    }
}

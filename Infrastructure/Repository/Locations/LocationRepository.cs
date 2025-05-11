namespace SLAPScheduling.Infrastructure.Repository.Locations
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(SLAPDbContext context) : base(context)
        {
        }

        public async Task<List<Location>> GetAllLocations()
        {
            var locations = await _context.Locations
                .Include(s => s.properties)
                .AsNoTracking()
                .ToListAsync();

            return locations;
        }

        public async Task<List<Location>> GetLocationsByWarehouseId(string warehouseId)
        {
            return await _context.Locations
                .Include(x => x.properties)
                .Include(x => x.materialSubLots)
                    .ThenInclude(x => x.materialLot)
                        .ThenInclude(x => x.material)
                            .ThenInclude(x => x.properties)
                .Where(x => x.warehouseId == warehouseId).ToListAsync();
        }
    }
}

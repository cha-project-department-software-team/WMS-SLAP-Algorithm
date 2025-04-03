namespace SLAPScheduling.Infrastructure.Repository.Warehouses
{
    public class WarehouseRepository : BaseRepository, IWarehouseRepository
    {
        public WarehouseRepository(SLAPDbContext context) : base(context)
        {
        }

        public async Task<List<Warehouse>> GetAllWarehouses()
        {
            var warehouses = await _context.Warehouses
                .Include(s => s.locations)
                    .ThenInclude(s => s.properties)
                .AsNoTracking()
                .ToListAsync();

            return warehouses;
        }
    }   
}

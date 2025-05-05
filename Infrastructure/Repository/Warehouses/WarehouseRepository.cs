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

        public async Task<Warehouse> GetWarehouseById(string warehouseId)
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(x => x.warehouseId == warehouseId);
            return warehouse is not null ? warehouse : throw new Exception($"There is no existing warehouse with warehouseId = {warehouseId}");
        }

        public async Task<Warehouse> GetWarehouseByIdAsync(string id)
        {
            var warehouse = await _context.Warehouses
                .Include(x => x.locations)
                .Include(x => x.properties)
                .FirstOrDefaultAsync(x => x.warehouseId == id);

            return warehouse;
        }
    }   
}

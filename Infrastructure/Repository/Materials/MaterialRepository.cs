namespace SLAPScheduling.Infrastructure.Repository.Materials
{
    public class MaterialRepository : BaseRepository, IMaterialRepository
    {
        public MaterialRepository(SLAPDbContext context) : base(context)
        {
        }

        public async Task<List<Material>> GetAllAsync()
        {
            var materials = await _context.Materials
                                .Include(x => x.properties)
                                .AsNoTracking()
                                .ToListAsync();

            return materials;
        }

        public async Task<Material> GetByIdAsync(string materialId)
        {
            var material = await _context.Materials
                .Include(s => s.properties)
                .FirstOrDefaultAsync(x => x.materialId == materialId);

            return material;
        }
    }
}

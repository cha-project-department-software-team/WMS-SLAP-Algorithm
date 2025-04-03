using SLAPScheduling.Domain.InterfaceRepositories.IMaterial;

namespace SLAPScheduling.Infrastructure.Repository.Materials
{
    public class MaterialPropertyRepository : BaseRepository, IMaterialPropertyRepository
    {
        public MaterialPropertyRepository(SLAPDbContext context) : base(context)
        {
        }

        public async Task<MaterialProperty> GetByIdAsync(string materialPropertyId)
        {
            var materialProperty = await _context.MaterialProperties.FirstOrDefaultAsync(x => x.propertyId == materialPropertyId);
            return materialProperty ?? throw new Exception("No data for material property");
        }
    }
}

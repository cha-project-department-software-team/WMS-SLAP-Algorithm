namespace SLAPScheduling.Domain.InterfaceRepositories.ILocation
{
    public interface IMaterialRepository : IRepository<Material>
    {
        Task<List<Material>> GetAllAsync();
        Task<Material> GetByIdAsync(string materialId);
    }
}

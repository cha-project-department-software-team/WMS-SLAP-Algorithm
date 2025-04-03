namespace SLAPScheduling.Domain.InterfaceRepositories.IMaterial
{
    public interface IMaterialPropertyRepository : IRepository<MaterialProperty>
    {
        Task<MaterialProperty> GetByIdAsync(string materialPropertyId);
    }
}

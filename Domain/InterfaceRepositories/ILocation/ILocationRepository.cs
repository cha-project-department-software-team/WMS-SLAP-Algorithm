namespace SLAPScheduling.Domain.InterfaceRepositories.ILocation
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<List<Location>> GetAllLocations();
    }
}

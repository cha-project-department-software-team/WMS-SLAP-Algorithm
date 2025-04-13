namespace SLAPScheduling.Domain.InterfaceRepositories.IInventoryIssues
{
    public interface IIssueLotRepository : IRepository<IssueLot>
    {
        Task<List<IssueLot>> GetAllAsync();
    }
}

namespace SLAPScheduling.Domain.InterfaceRepositories.IScheduling
{
    public interface IIssueSchedulingRepository
    {
        Task<List<IssueSublot>> Execute(string warehouseId, string issueLotStatus);
    }
}

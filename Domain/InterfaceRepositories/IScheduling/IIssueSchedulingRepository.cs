namespace SLAPScheduling.Domain.InterfaceRepositories.IScheduling
{
    public interface IIssueSchedulingRepository
    {
        Task<List<(IssueSublot SubLot, double StoragePercentage)>> Execute(string warehouseId);
    }
}

namespace SLAPScheduling.Domain.InterfaceRepositories.IScheduling
{
    public interface IReceiptSchedulingRepository
    {
        Task<List<(ReceiptSublot SubLot, double StoragePercentage)>> Execute(string warehouseId, AlgorithmType algorithmType);
    }
}

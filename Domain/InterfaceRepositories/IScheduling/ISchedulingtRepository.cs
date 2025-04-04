namespace SLAPScheduling.Domain.InterfaceRepositories.IScheduling
{
    public interface ISchedulingRepository
    {
        Task<List<ReceiptSublot>> Execute(string warehouseId, string receiptLotStatus);
    }
}

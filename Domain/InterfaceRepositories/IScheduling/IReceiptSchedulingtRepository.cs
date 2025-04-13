namespace SLAPScheduling.Domain.InterfaceRepositories.IScheduling
{
    public interface IReceiptSchedulingRepository
    {
        Task<List<ReceiptSublot>> Execute(string warehouseId, string receiptLotStatus);
    }
}

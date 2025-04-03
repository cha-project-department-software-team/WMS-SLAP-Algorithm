namespace SLAPScheduling.Domain.InterfaceRepositories.IInventoryReceipts
{
    public interface IReceiptLotRepository : IRepository<ReceiptLot>
    {
        Task<List<ReceiptLot>> GetAllAsync();
    }
}

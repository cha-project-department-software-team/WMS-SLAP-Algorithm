namespace SLAPScheduling.Domain.InterfaceRepositories.IInventoryReceipts
{
    public interface IInventoryReceiptRepository : IRepository<InventoryReceipt>
    {
        Task<IEnumerable<InventoryReceipt>> GetAll();
    }
}

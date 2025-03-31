using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;

namespace SLAPScheduling.Domain.InterfaceRepositories.IInventortReceipts
{
    public interface IInventortReceiptRepository : IRepository<InventoryReceipt>
    {
        Task<IEnumerable<InventoryReceipt>> GetAll();


    }
}

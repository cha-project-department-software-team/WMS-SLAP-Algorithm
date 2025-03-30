using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;

namespace SLAPScheduling.Domain.InterfaceRepositories.IScheduling
{
    public interface ISchedulingRepository
    {
        List<ReceiptSublot> Execute(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials);
    }
}

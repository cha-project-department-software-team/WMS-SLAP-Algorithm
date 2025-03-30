using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using SLAP.AggregateModels.StorageAggregate;

namespace SLAPScheduling.AggregateModels.SchedulingAggregate
{
    public interface ISchedulingtRepository
    {
        List<ReceiptSubLot> Implement(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials);
    }
}

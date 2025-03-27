using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.JobInforAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using SLAP.AggregateModels.StorageAggregate;

namespace SLAP.AggregateModels.InputAggregate
{
    public interface IObjectInputRepository
    {
        List<ReceiptSubLot> Implement(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials);
    }
}

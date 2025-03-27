using MediatR;
using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.JobInforAggregate;
using SLAP.AggregateModels.MaterialAggregate;
using SLAP.AggregateModels.StorageAggregate;

namespace SLAP.Commands.Inputs
{
    public record AddSchedulingCommand(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials) : IRequest<List<ReceiptSubLot>>;
}

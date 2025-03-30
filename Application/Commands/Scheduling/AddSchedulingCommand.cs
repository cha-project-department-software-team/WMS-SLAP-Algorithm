using MediatR;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;

namespace SLAPScheduling.Application.Commands.Scheduling
{
    public record AddSchedulingCommand(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials) : IRequest<List<ReceiptSublot>>;
}

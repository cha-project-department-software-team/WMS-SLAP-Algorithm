namespace SLAPScheduling.Application.Commands.Scheduling
{
    public record AddSchedulingCommand(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials) : IRequest<List<ReceiptSublot>>;
}

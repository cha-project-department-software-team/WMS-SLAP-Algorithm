namespace SLAPScheduling.Domain.InterfaceRepositories.IScheduling
{
    public interface ISchedulingRepository
    {
        List<ReceiptSublot> Execute(InventoryReceipt inventoryReceipt, Warehouse warehouse, List<Material> materials);
    }
}

namespace SLAPScheduling.Domain.InterfaceRepositories.IWarehouse
{
    public interface IWarehouseRepository
    {
        Task<List<Warehouse>> GetAllWarehouses();
        Task<Warehouse> GetWarehouseById(string warehouseId);
    }
}

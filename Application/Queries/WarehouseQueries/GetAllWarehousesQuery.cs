using SLAPScheduling.Application.DTOs.WarehouseDTOs;

namespace SLAPScheduling.Application.Queries.WarehouseQueries
{
    public class GetAllWarehousesQuery : IRequest<IEnumerable<WarehouseDTO>>
    {
        public GetAllWarehousesQuery()
        {
        }
    }
}

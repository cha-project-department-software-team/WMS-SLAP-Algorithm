using SLAPScheduling.Application.DTOs.WarehouseDTOs;

namespace SLAPScheduling.Application.Queries.WarehouseQueries
{
    public class GetAllWarehousesQueryHandler : IRequestHandler<GetAllWarehousesQuery, IEnumerable<WarehouseDTO>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;

        public GetAllWarehousesQueryHandler(IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WarehouseDTO>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
        {
            var warehouses = await _warehouseRepository.GetAllWarehouses();
            if (warehouses.Count == 0)
            {
                throw new Exception("No result for Warehouses");
            }

            var warehouseDTOs = new List<WarehouseDTO>();
            foreach (var warehouse in warehouses)
            {
                var warehouseDTO = _mapper.Map<WarehouseDTO>(warehouse);
                warehouseDTOs.Add(warehouseDTO);
            }

            return warehouseDTOs;
        }



    }
}

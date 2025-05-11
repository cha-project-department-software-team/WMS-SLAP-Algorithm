using SLAPScheduling.Application.DTOs.ReceiptResults;
using SLAPScheduling.Application.Exceptions;

namespace SLAPScheduling.Application.Queries.ReceiptScheduling
{
    public class ReceiptLayoutSchedulingQueryHandler : IRequestHandler<ReceiptLayoutSchedulingQuery, IEnumerable<LocationRDTO>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IReceiptSchedulingRepository _schedulingRepository;
        private readonly IMapper _mapper;

        public ReceiptLayoutSchedulingQueryHandler(IWarehouseRepository warehouseRepository, ILocationRepository locationRepository, IReceiptSchedulingRepository schedulingRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _locationRepository = locationRepository;
            _schedulingRepository = schedulingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LocationRDTO>> Handle(ReceiptLayoutSchedulingQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(request.WarehouseId);
            if (warehouse is null)
                throw new EntityNotFoundException(nameof(Warehouse), request.WarehouseId);

            var locations = await _locationRepository.GetLocationsByWarehouseId(request.WarehouseId);
            if (locations is null || locations.Count == 0)
                throw new EntityNotFoundException(nameof(Location), request.WarehouseId);

            var sublotResults = await _schedulingRepository.Execute(request.WarehouseId);
            if (sublotResults is null)
            {
                throw new Exception("No result for Storage Locations Assignment Problem");
            }

            var locationRDTOs = new List<LocationRDTO>();
            foreach (var location in locations)
            {
                var materialSubLotRDTOs = new List<MaterialSubLotRDTO>();
                if (location.materialSubLots?.Count > 0)
                {
                    materialSubLotRDTOs = location.materialSubLots.Select(x =>
                    {
                        var storagePercent = x.GetStoragePercentage(location);
                        var materialSublotRDTO = new MaterialSubLotRDTO(subLotId: x.subLotId,
                                                                        existingQuantity: x.existingQuality,
                                                                        storagePercentage: storagePercent <= 1.0f ? storagePercent : 1.0f,
                                                                        locationId: location.locationId,
                                                                        lotNumber: x.lotNumber);
                        return materialSublotRDTO;
                    }).ToList();
                }

                var receiptSubLotRDTOs = new List<ReceiptSubLotLayoutRDTO>();
                var sublots = sublotResults.Where(x => x.SubLot.locationId.Equals(location.locationId, StringComparison.OrdinalIgnoreCase));
                if (sublots?.Count() > 0)
                {
                    receiptSubLotRDTOs = sublots.Select(x =>
                    {
                        var receiptSubLotRDTO = new ReceiptSubLotLayoutRDTO(receiptSublotId: x.SubLot.receiptSublotId,
                                                                            lotNumber: x.SubLot.receiptLotId,
                                                                            importedQuantity: x.SubLot.importedQuantity,
                                                                            locationId: x.SubLot.locationId,
                                                                            storagePercentage: x.StoragePercentage <= 1.0f ? x.StoragePercentage : 1.0f);
                        return receiptSubLotRDTO;
                    }).ToList();
                }
                          
                var locationRDTO = new LocationRDTO(locationId: location.locationId,
                                                    materialSubLots: materialSubLotRDTOs,
                                                    receiptSubLots: receiptSubLotRDTOs);   
                locationRDTOs.Add(locationRDTO);
            }

            return locationRDTOs;
        }
    }
}

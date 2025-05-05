using SLAPScheduling.Application.DTOs.IssueResults;
using SLAPScheduling.Application.DTOs.LocationDTOs;
using SLAPScheduling.Application.DTOs.ReceiptResults;
using SLAPScheduling.Application.Exceptions;

namespace SLAPScheduling.Application.Queries.IssueScheduling
{
    public class IssueLayoutSchedulingQueryHandler : IRequestHandler<IssueLayoutSchedulingQuery, IEnumerable<LocationIDTO>>
    {

        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IIssueSchedulingRepository _schedulingRepository;
        private readonly IMapper _mapper;

        public IssueLayoutSchedulingQueryHandler(IWarehouseRepository warehouseRepository, ILocationRepository locationRepository, IIssueSchedulingRepository schedulingRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _locationRepository = locationRepository;
            _schedulingRepository = schedulingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LocationIDTO>> Handle(IssueLayoutSchedulingQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepository.GetWarehouseByIdAsync(request.WarehouseId);
            if (warehouse is null)
                throw new EntityNotFoundException(nameof(Warehouse), request.WarehouseId);

            var locations = await _locationRepository.GetLocationsByWarehouseId(request.WarehouseId);
            if (locations is null || locations.Count == 0)
                throw new EntityNotFoundException(nameof(Location), request.WarehouseId);

            var sublotResults = await _schedulingRepository.Execute(request.WarehouseId);
            if (sublotResults is null || sublotResults.Count == 0)
            {
                throw new Exception("No result for retrieving issue sublots");
            }

            var locationIDTOs = new List<LocationIDTO>();
            foreach (var location in locations)
            {
                var materialSubLotIDTOs = new List<MaterialSubLotIDTO>();
                if (location.materialSubLots?.Count > 0)
                {
                    materialSubLotIDTOs = location.materialSubLots.Select(x => new MaterialSubLotIDTO(subLotId: x.subLotId,
                                                                                                      existingQuantity: x.existingQuality,
                                                                                                      storagePercentage: x.GetStoragePercentage(location),
                                                                                                      locationId: location.locationId,
                                                                                                      lotNumber: x.lotNumber)).ToList(); 
                }

                var issueSubLotIDTOs = new List<IssueSubLotLayoutIDTO>();
                var sublots = sublotResults.Where(x => x.SubLot.GetLocationId().Equals(location.locationId, StringComparison.OrdinalIgnoreCase));
                if (sublots?.Count() > 0)
                {
                    issueSubLotIDTOs = sublots.Select(x => new IssueSubLotLayoutIDTO(issueSublotId: x.SubLot.issueSublotId,
                                                                               lotNumber: x.SubLot.issueLotId,
                                                                               requestedQuantity: x.SubLot.requestedQuantity,
                                                                               locationId: x.SubLot.GetLocationId(),
                                                                               storagePercentage: x.StoragePercentage)).ToList();
                }

                var locationIDTO = new LocationIDTO(locationId: location.locationId,
                                                    materialSubLots: materialSubLotIDTOs,
                                                    issueSubLots: issueSubLotIDTOs);
                locationIDTOs.Add(locationIDTO);
            }

            return locationIDTOs;
        }
    }
}

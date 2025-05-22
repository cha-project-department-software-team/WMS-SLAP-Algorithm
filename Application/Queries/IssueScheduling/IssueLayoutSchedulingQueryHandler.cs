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

            var issueSublotResults = await _schedulingRepository.Execute(request.WarehouseId);
            if (issueSublotResults is null || issueSublotResults.Count == 0)
            {
                throw new Exception("No result for retrieving issue sublots");
            }

            var locationIDTOs = new List<LocationIDTO>();
            foreach (var location in locations)
            {
                var issueSubLotIDTOs = new List<IssueSubLotLayoutIDTO>();
                var issueSublots = issueSublotResults.Where(x => x.SubLot.GetLocationId().Equals(location.locationId, StringComparison.OrdinalIgnoreCase));
                if (issueSublots?.Count() > 0)
                {
                    issueSubLotIDTOs = issueSublots.Select(x =>
                    {
                        var issueSubLotIDTO = new IssueSubLotLayoutIDTO(issueSublotId: x.SubLot.issueSublotId,
                                                                        lotNumber: x.SubLot.materialSublot.lotNumber,
                                                                        requestedQuantity: x.SubLot.requestedQuantity,
                                                                        locationId: x.SubLot.GetLocationId(),
                                                                        storagePercentage: x.StoragePercentage <= 1.0f ? x.StoragePercentage : 1.0f);
                        return issueSubLotIDTO;
                    }).ToList();
                }

                var materialSubLotIDTOs = new List<MaterialSubLotIDTO>();
                if (location.materialSubLots?.Count > 0)
                {
                    foreach (var materialSublot in location.materialSubLots)
                    {
                        var lotNumber = materialSublot.lotNumber;
                        var getIssueSublots = issueSubLotIDTOs?.Count > 0 ? issueSubLotIDTOs.Where(x => x.LotNumber.Equals(lotNumber)).ToList() : null;
                        if (getIssueSublots?.Count > 0)
                        {
                            materialSublot.existingQuality -= getIssueSublots.Sum(x => x.RequestedQuantity);
                        }

                        if (materialSublot.existingQuality > 0)
                        {
                            var storagePercent = materialSublot.GetStoragePercentage(location);
                            var materialSubLotIDTO = new MaterialSubLotIDTO(subLotId: materialSublot.subLotId,
                                                                            existingQuantity: materialSublot.existingQuality,
                                                                            storagePercentage: storagePercent <= 1.0f ? storagePercent : 1.0f,
                                                                            locationId: location.locationId,
                                                                            lotNumber: lotNumber);
                            materialSubLotIDTOs.Add(materialSubLotIDTO);
                        }
                    }
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

using SLAPScheduling.Application.DTOs.IssueResults;
using SLAPScheduling.Application.DTOs.LocationDTOs;
using SLAPScheduling.Application.DTOs.ReceiptResults;
using SLAPScheduling.Application.Exceptions;

namespace SLAPScheduling.Application.Queries.IssueScheduling
{
    public class IssueDetailSchedulingQueryHandler : IRequestHandler<IssueDetailSchedulingQuery, IEnumerable<IssueSubLotDetailIDTO>>
    {
        private readonly IIssueSchedulingRepository _schedulingRepository;
        private readonly IMapper _mapper;

        public IssueDetailSchedulingQueryHandler(IIssueSchedulingRepository schedulingRepository, IMapper mapper)
        {
            _schedulingRepository = schedulingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IssueSubLotDetailIDTO>> Handle(IssueDetailSchedulingQuery request, CancellationToken cancellationToken)
        {
            var sublotResults = await _schedulingRepository.Execute(request.WarehouseId);
            if (sublotResults is null || sublotResults.Count == 0)
            {
                throw new Exception("No result for retrieving issue sublots");
            }

            var issueSubLotDetailRDTOs = sublotResults.Select(x => new IssueSubLotDetailIDTO(issueSublotId: x.SubLot.issueSublotId,
                                                                                                 materialId: x.SubLot.GetMaterialId(),
                                                                                                 materialName: x.SubLot.GetMaterialName(),
                                                                                                 requestedQuantity: x.SubLot.requestedQuantity,
                                                                                                 storagePercentage: x.StoragePercentage,
                                                                                                 locationId: x.SubLot.GetLocationId(),
                                                                                                 lotNumber: x.SubLot.GetLotNumber()));
            return issueSubLotDetailRDTOs;
        }
    }
}

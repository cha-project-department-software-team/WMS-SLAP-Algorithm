namespace SLAPScheduling.Application.Queries.IssueScheduling
{
    public class IssueSchedulingQueryHandler : IRequestHandler<IssueSchedulingQuery, IEnumerable<IssueSubLotDTO>>
    {
        private readonly IIssueSchedulingRepository _schedulingRepository;
        private readonly IMapper _mapper;

        public IssueSchedulingQueryHandler(IIssueSchedulingRepository schedulingRepository, IMapper mapper)
        {
            _schedulingRepository = schedulingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IssueSubLotDTO>> Handle(IssueSchedulingQuery request, CancellationToken cancellationToken)
        {
            var issueSubLots = await _schedulingRepository.Execute(request.WarehouseId, request.IssueLotStatus);
            if (issueSubLots is null || issueSubLots.Count == 0)
            {
                throw new Exception("No result for retrieving issue sublots");
            }

            var issueSubLotDTOs = new List<IssueSubLotDTO>();
            foreach (var issueSubLot in issueSubLots)
            {
                var issueSubLotDTO = _mapper.Map<IssueSubLotDTO>(issueSubLot);
                issueSubLotDTOs.Add(issueSubLotDTO);
            }

            return issueSubLotDTOs;
        }
    }
}

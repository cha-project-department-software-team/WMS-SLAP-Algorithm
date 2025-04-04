namespace SLAPScheduling.Application.Queries.Scheduling
{
    public class SchedulingQueryHandler : IRequestHandler<SchedulingQuery, IEnumerable<ReceiptSubLotDTO>>
    {
        private readonly ISchedulingRepository _schedulingRepository;
        private readonly IMapper _mapper;

        public SchedulingQueryHandler(ISchedulingRepository schedulingRepository, IMapper mapper)
        {
            _schedulingRepository = schedulingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReceiptSubLotDTO>> Handle(SchedulingQuery request, CancellationToken cancellationToken)
        {
            var receiptSubLots = await _schedulingRepository.Execute(request.WarehouseId, request.ReceiptLotStatus);
            if (receiptSubLots == null || receiptSubLots.Count == 0)
            {
                throw new Exception("No result for Storage Locations Assignment Problem");
            }

            var receiptSubLotDTOs = new List<ReceiptSubLotDTO>();
            foreach (var receiptSubLot in receiptSubLots)
            {
                var receiptSubLotDTO = _mapper.Map<ReceiptSubLotDTO>(receiptSubLot);
                receiptSubLotDTOs.Add(receiptSubLotDTO);
            }

            return receiptSubLotDTOs;
        }
    }
}

namespace SLAPScheduling.Application.Queries.ReceiptScheduling
{
    public class ReceiptSchedulingQueryHandler : IRequestHandler<ReceiptSchedulingQuery, IEnumerable<ReceiptSubLotDTO>>
    {
        private readonly IReceiptSchedulingRepository _schedulingRepository;
        private readonly IMapper _mapper;

        public ReceiptSchedulingQueryHandler(IReceiptSchedulingRepository schedulingRepository, IMapper mapper)
        {
            _schedulingRepository = schedulingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReceiptSubLotDTO>> Handle(ReceiptSchedulingQuery request, CancellationToken cancellationToken)
        {
            var receiptSubLots = await _schedulingRepository.Execute(request.WarehouseId, request.ReceiptLotStatus);
            if (receiptSubLots is null || receiptSubLots.Count == 0)
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

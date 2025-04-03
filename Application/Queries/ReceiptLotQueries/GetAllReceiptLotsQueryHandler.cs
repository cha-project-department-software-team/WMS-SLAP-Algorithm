namespace SLAPScheduling.Application.Queries.ReceiptLotQueries
{
    public class GetAllReceiptLotsQueryHandler : IRequestHandler<GetAllReceiptLotsQuery, IEnumerable<ReceiptLotDTO>>
    {
        private readonly IReceiptLotRepository _receiptLotRepository;
        private readonly IMapper _mapper;

        public GetAllReceiptLotsQueryHandler(IReceiptLotRepository receiptLotRepository, IMapper mapper)
        {
            _receiptLotRepository = receiptLotRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReceiptLotDTO>> Handle(GetAllReceiptLotsQuery request, CancellationToken cancellationToken)
        {
            var receiptLots = await _receiptLotRepository.GetAllAsync();
            if (receiptLots.Count == 0)
            {
                throw new Exception("No result for receipt Lots");
            }

            var receiptLotDTOs = new List<ReceiptLotDTO>();
            foreach (var receiptLot in receiptLots)
            {
                var receiptLotDTO = _mapper.Map<ReceiptLotDTO>(receiptLot);
                receiptLotDTOs.Add(receiptLotDTO);
            }

            return receiptLotDTOs;
        }



    }
}

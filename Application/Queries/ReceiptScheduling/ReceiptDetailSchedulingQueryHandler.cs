using SLAPScheduling.Application.DTOs.ReceiptResults;

namespace SLAPScheduling.Application.Queries.ReceiptScheduling
{
    public class ReceiptDetailSchedulingQueryHandler : IRequestHandler<ReceiptDetailSchedulingQuery, IEnumerable<ReceiptSubLotDetailRDTO>>
    {
        private readonly IReceiptSchedulingRepository _schedulingRepository;
        private readonly IMapper _mapper;

        public ReceiptDetailSchedulingQueryHandler(IReceiptSchedulingRepository schedulingRepository, IMapper mapper)
        {
            _schedulingRepository = schedulingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReceiptSubLotDetailRDTO>> Handle(ReceiptDetailSchedulingQuery request, CancellationToken cancellationToken)
        {
            var sublotResults = await _schedulingRepository.Execute(request.WarehouseId);
            if (sublotResults is null || sublotResults.Count == 0)
            {
                throw new Exception("No result for Storage Locations Assignment Problem");
            }

            var receiptSubLotDetailRDTOs = sublotResults.Select(x => new ReceiptSubLotDetailRDTO(receiptSublotId: x.SubLot.receiptSublotId,
                                                                                                 materialId: x.SubLot.GetMaterialId(),
                                                                                                 materialName: x.SubLot.GetMaterialName(),
                                                                                                 importedQuantity: x.SubLot.importedQuantity,
                                                                                                 storagePercentage: x.StoragePercentage,
                                                                                                 locationId: x.SubLot.locationId,
                                                                                                 lotNumber: x.SubLot.receiptLotId));
            return receiptSubLotDetailRDTOs;
        }
    }
}

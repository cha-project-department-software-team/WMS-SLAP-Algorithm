using AutoMapper;
using MediatR;
using SLAPScheduling.Application.DTOs.InventoryReceiptDTOs;
using SLAPScheduling.Domain.InterfaceRepositories.IScheduling;

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
            var receiptSubLots = _schedulingRepository.Execute(null, null, null);
            if (receiptSubLots == null || receiptSubLots.Count == 0)
            {
                throw new Exception("No result for Storage Locations Assignement Problem");
            }

            var receipSubLotDTOs = new List<ReceiptSubLotDTO>();
            foreach (var receiptSubLot in receiptSubLots)
            {
                var receiptSubLotDTO = _mapper.Map<ReceiptSubLotDTO>(receiptSubLot);
                receipSubLotDTOs.Add(receiptSubLotDTO);
            }

            return new List<ReceiptSubLotDTO>();
        }
    }
}

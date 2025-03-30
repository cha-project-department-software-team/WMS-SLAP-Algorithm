using MediatR;
using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.AggregateModels.SchedulingAggregate;

namespace SLAP.Commands.Inputs
{
    public class AddSchedulingHandler : IRequestHandler<AddSchedulingCommand, List<ReceiptSubLot>>
    {
        private readonly ISchedulingtRepository _schedulingRepository;

        public AddSchedulingHandler(ISchedulingtRepository schedulingRepository)
        {
            _schedulingRepository = schedulingRepository;
        }

        /// <summary>
        /// Create a POST API to implement the scheduling based on the input data.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<ReceiptSubLot>> Handle(AddSchedulingCommand request, CancellationToken cancellationToken)
        {
            List<ReceiptSubLot> newListJobInfor = _schedulingRepository.Implement(request.inventoryReceipt, request.warehouse, request.materials);

            return Task.FromResult(newListJobInfor);
        }
    }
}

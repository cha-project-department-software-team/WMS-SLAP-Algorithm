using MediatR;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.InterfaceRepositories.IScheduling;

namespace SLAPScheduling.Application.Commands.Scheduling
{
    public class AddSchedulingHandler : IRequestHandler<AddSchedulingCommand, List<ReceiptSublot>>
    {
        private readonly ISchedulingRepository _schedulingRepository;

        public AddSchedulingHandler(ISchedulingRepository schedulingRepository)
        {
            _schedulingRepository = schedulingRepository;
        }

        /// <summary>
        /// Create a POST API to implement the scheduling based on the input data.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<ReceiptSublot>> Handle(AddSchedulingCommand request, CancellationToken cancellationToken)
        {
            List<ReceiptSublot> newListJobInfor = _schedulingRepository.Execute(request.inventoryReceipt, request.warehouse, request.materials);

            return Task.FromResult(newListJobInfor);
        }
    }
}

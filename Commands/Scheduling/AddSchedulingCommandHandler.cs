using MediatR;
using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.InventoryReceiptAggregate;

namespace SLAP.Commands.Inputs
{
    public class AddSchedulingHandler : IRequestHandler<AddSchedulingCommand, List<ReceiptSubLot>>
    {
        private readonly IObjectInputRepository _objectInputRepository;

        public AddSchedulingHandler(IObjectInputRepository objectInputRepository)
        {
            _objectInputRepository = objectInputRepository;
        }

        /// <summary>
        /// Create a POST API to implement the scheduling based on the input data.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<ReceiptSubLot>> Handle(AddSchedulingCommand request, CancellationToken cancellationToken)
        {
            List<ReceiptSubLot> newListJobInfor = _objectInputRepository.Implement(request.inventoryReceipt, request.warehouse, request.materials);

            return Task.FromResult(newListJobInfor);
        }
    }
}

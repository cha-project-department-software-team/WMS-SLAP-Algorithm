using MediatR;
using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.JobInforAggregate;

namespace SLAP.Commands.Inputs
{
    public class AddSchedulingHandler : IRequestHandler<AddSchedulingCommand, List<JobInfor>>
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
        public Task<List<JobInfor>> Handle(AddSchedulingCommand request, CancellationToken cancellationToken)
        {
            List<JobInfor> newListJobInfor = _objectInputRepository.Implement(request.input);

            return Task.FromResult(newListJobInfor);
        }
    }
}

using MediatR;
using TabuSearchImplement.AggregateModels.InputAggregate;
using TabuSearchImplement.AggregateModels.JobInforAggregate;
using TabuSearchImplement.Commands.Inputs;

namespace TabuSearchImplement.Commands.Inputs
{
    public class AddInputHandler : IRequestHandler<AddInputCommand, List<JobInfor>>
    {
        private readonly IObjectInputRepository _objectInputRepository;

        public AddInputHandler(IObjectInputRepository objectInputRepository)
        {
            _objectInputRepository = objectInputRepository;
        }

        public Task<List<JobInfor>> Handle(AddInputCommand request, CancellationToken cancellationToken)
        {
            List<JobInfor> newListJobInfor = _objectInputRepository.Implement(request.input);

            return Task.FromResult(newListJobInfor);
        }
    }
}

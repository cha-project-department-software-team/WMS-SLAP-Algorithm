using MediatR;
using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.JobInforAggregate;

namespace SLAP.Commands.Inputs
{
    public record AddSchedulingCommand(ObjectInput input) : IRequest<List<JobInfor>>;
}

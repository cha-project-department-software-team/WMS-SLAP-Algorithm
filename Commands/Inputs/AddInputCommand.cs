using MediatR;
using SLAP.AggregateModels.DeviceAggregate;
using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.JobInforAggregate;

namespace SLAP.Commands.Inputs
{
    public record AddInputCommand(ObjectInput input) : IRequest<List<JobInfor>>;
}

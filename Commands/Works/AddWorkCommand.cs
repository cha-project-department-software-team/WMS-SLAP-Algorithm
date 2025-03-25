using MediatR;
using SLAP.AggregateModels.WorkAggregate;

namespace SLAP.Commands.Works
{
    public record AddWorkCommand(WorkInputs works) : IRequest<WorkInputs>;
}

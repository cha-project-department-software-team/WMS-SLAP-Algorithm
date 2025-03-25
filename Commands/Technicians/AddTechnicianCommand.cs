using MediatR;
using SLAP.AggregateModels.TechnicianAggregate;

namespace SLAP.Commands.Technicians
{
    public record AddTechnicianCommand(TechnicianInputs technicians) : IRequest<TechnicianInputs>;
}

using MediatR;
using SLAP.AggregateModels.MaterialAggregate;

namespace SLAP.Commands.Materials
{
    public record AddMaterialCommand(MaterialInputs materials) : IRequest<MaterialInputs>;
}

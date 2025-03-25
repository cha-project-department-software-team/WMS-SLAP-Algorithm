using MediatR;
using SLAP.AggregateModels.WareHouseMaterialAggregate;

namespace SLAP.Commands.WareHouseMaterials
{
    public record AddWareHouseMaterialCommand(WareHouseMaterialInputs wareHouseMaterials) : IRequest<WareHouseMaterialInputs>;
}

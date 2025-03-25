using MediatR;
using SLAP.AggregateModels.MaterialAggregate;
using SLAP.Commands.Materials;

namespace SLAP.Commands.Materials
{
    public class AddMaterialHandler : IRequestHandler<AddMaterialCommand, MaterialInputs>
    {
        private readonly IMaterialObjectInputRepository _materialObjectInputRepository;

        public AddMaterialHandler(IMaterialObjectInputRepository materialObjectInputRepository)
        {
            _materialObjectInputRepository = materialObjectInputRepository;
        }

        public Task<MaterialInputs> Handle(AddMaterialCommand request, CancellationToken cancellationToken)
        {
            var newListMaterial = new List<MaterialObjectInput>();
            foreach (MaterialObjectInput materialObject in request.materials.JsonInput)
            {
                var material = _materialObjectInputRepository.Add(materialObject);
                newListMaterial.Add(material);
            }

            MaterialInputs materialInputs = new MaterialInputs(newListMaterial.ToArray());
            return Task.FromResult(materialInputs);
        }
    }
}

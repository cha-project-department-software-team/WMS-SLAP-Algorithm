using MediatR;
using Microsoft.AspNetCore.Mvc;
using SLAP.AggregateModels.MaterialAggregate;
using SLAP.Commands.Materials;

namespace SLAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaterialsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<MaterialInputs> Post(MaterialInputs materials)
        {
            return await _mediator.Send(new AddMaterialCommand(materials));
        }
    }
}

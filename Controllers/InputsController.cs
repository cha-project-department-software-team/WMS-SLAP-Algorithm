using MediatR;
using Microsoft.AspNetCore.Mvc;
using SLAP.AggregateModels.InputAggregate;
using SLAP.AggregateModels.JobInforAggregate;
using SLAP.Commands.Inputs;

namespace SLAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InputsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<List<JobInfor>> Post(ObjectInput input)
        {
            return await _mediator.Send(new AddInputCommand(input));
        }
    }
}

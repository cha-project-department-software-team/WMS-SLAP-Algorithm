using MediatR;
using Microsoft.AspNetCore.Mvc;
using SLAP.AggregateModels.TechnicianAggregate;
using SLAP.Commands.Technicians;

namespace SLAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniciansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TechniciansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<TechnicianInputs> Post(TechnicianInputs works)
        {
            return await _mediator.Send(new AddTechnicianCommand(works));
        }
    }
}

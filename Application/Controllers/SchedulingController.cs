using MediatR;
using Microsoft.AspNetCore.Mvc;
using SLAPScheduling.Application.Commands.Scheduling;
using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;

namespace SLAPScheduling.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SchedulingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<List<ReceiptSublot>> Post([FromBody] AddSchedulingCommand command)
        { 
            return await _mediator.Send(command);
        }
    }
}

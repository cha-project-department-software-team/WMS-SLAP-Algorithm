using MediatR;
using Microsoft.AspNetCore.Mvc;
using SLAP.AggregateModels.DeviceAggregate;
using SLAP.Commands.Devices;

namespace SLAP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DevicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<DeviceInputs> Post(DeviceInputs devices)
        {
            return await _mediator.Send(new AddDeviceCommand(devices));
        }
    }
}

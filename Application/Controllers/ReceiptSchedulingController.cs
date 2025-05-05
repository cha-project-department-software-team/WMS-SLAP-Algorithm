using SLAPScheduling.Application.DTOs.ReceiptResults;

namespace SLAPScheduling.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptSchedulingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReceiptSchedulingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetReceiptScheduling")]
        public async Task<IEnumerable<LocationRDTO>> GetReceiptScheduling(string warehouseId = "TP01")
        {
            var query = new ReceiptSchedulingQuery(warehouseId);

            return await _mediator.Send(query);
        }
    }
}

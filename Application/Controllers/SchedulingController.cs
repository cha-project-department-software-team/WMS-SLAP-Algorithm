using SLAPScheduling.Application.Queries.Scheduling;

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

        [HttpGet("GetSchedulingResult")]
        public async Task<IEnumerable<ReceiptSubLotDTO>> GetSchedulingResult(string warehouseId = "TP01", string receiptLotStatus = "Pending")
        {
            var query = new SchedulingQuery(warehouseId, receiptLotStatus);

            return await _mediator.Send(query);
        }
    }
}

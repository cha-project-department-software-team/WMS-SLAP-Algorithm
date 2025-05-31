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

        [HttpGet("GetReceiptLayoutScheduling")]
        public async Task<IEnumerable<LocationRDTO>> GetReceiptLayoutScheduling(string warehouseId = "BB01", string algorithmType = "TabuSearch")
        {
            var query = new ReceiptLayoutSchedulingQuery(warehouseId, algorithmType);

            return await _mediator.Send(query);
        }

        [HttpGet("GetReceiptDetailScheduling")]
        public async Task<IEnumerable<ReceiptSubLotDetailRDTO>> GetReceiptDetailScheduling(string warehouseId = "BB01", string algorithmType = "TabuSearch")
        {
            var query = new ReceiptDetailSchedulingQuery(warehouseId, algorithmType);

            return await _mediator.Send(query);
        }
    }
}

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
        public async Task<IEnumerable<ReceiptSubLotDTO>> GetReceiptScheduling(string warehouseId = "TP01", string receiptLotStatus = "Pending")
        {
            var query = new ReceiptSchedulingQuery(warehouseId, receiptLotStatus);

            return await _mediator.Send(query);
        }
    }
}

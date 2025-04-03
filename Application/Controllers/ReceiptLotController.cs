namespace SLAPScheduling.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptLotController : ControllerBase
    {
        private IMediator _mediator;

        public ReceiptLotController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllReceiptLots")]
        public async Task<IEnumerable<ReceiptLotDTO>> GetAllReceiptLots()
        {
            var query = new GetAllReceiptLotsQuery();

            return await _mediator.Send(query);
        }

    }
}

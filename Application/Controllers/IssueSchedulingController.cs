namespace SLAPScheduling.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueSchedulingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IssueSchedulingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetIssueScheduling")]
        public async Task<IEnumerable<IssueSubLotDTO>> GetReceiptScheduling(string warehouseId = "TP01", string issueLotStatus = "Pending")
        {
            var query = new IssueSchedulingQuery(warehouseId, issueLotStatus);

            return await _mediator.Send(query);
        }
    }
}

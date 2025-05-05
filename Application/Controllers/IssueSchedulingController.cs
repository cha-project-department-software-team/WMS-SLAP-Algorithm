using SLAPScheduling.Application.DTOs.IssueResults;

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
        public async Task<IEnumerable<LocationIDTO>> GetReceiptScheduling(string warehouseId = "TP01")
        {
            var query = new IssueSchedulingQuery(warehouseId);

            return await _mediator.Send(query);
        }
    }
}

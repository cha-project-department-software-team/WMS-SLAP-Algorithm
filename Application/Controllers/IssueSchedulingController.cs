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

        [HttpGet("GetIssueLayoutScheduling")]
        public async Task<IEnumerable<LocationIDTO>> GetReceiptScheduling(string warehouseId = "BB01")
        {
            var query = new IssueLayoutSchedulingQuery(warehouseId);

            return await _mediator.Send(query);
        }

        [HttpGet("GetIssueDetailScheduling")]
        public async Task<IEnumerable<IssueSubLotDetailIDTO>> GetIssueDetailScheduling(string warehouseId = "BB01")
        {
            var query = new IssueDetailSchedulingQuery(warehouseId);

            return await _mediator.Send(query);
        }
    }
}

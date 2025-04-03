namespace SLAPScheduling.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private IMediator _mediator;

        public WarehouseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllWarehouses")]
        public async Task<IEnumerable<WarehouseDTO>> GetAllWarehouses()
        {
            var query = new GetAllWarehousesQuery();

            return await _mediator.Send(query);
        }

    }
}

namespace SLAPScheduling.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private IMediator _mediator;

        public LocationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllLocations")]
        public async Task<IEnumerable<LocationDTO>> GetAllLocations()
        {
            var query = new GetAllLocationsQuery(); 

            return await _mediator.Send(query);
        }
    }
}

using SLAPScheduling.Application.Queries.MaterialQueries;

namespace SLAPScheduling.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private IMediator _mediator;

        public MaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllMaterials")]
        public async Task<IEnumerable<MaterialDTO>> GetAllMaterials()
        {
            var query = new GetAllMaterialQuery();

            return await _mediator.Send(query);
        }

        [HttpGet("GetMaterialById/{materialId}")]
        public async Task<MaterialDTO> GetById(string materialId)
        {
            var query = new GetMaterialByIdQuery(materialId);

            return await _mediator.Send(query);
        }

        [HttpGet("GetMaterialPropertyById/{propertyId}")]
        public async Task<MaterialPropertyDTO> GetPropertyById(string propertyId)
        {
            var query = new GetMaterialPropertyByIdQuery(propertyId);

            return await _mediator.Send(query);
        }
    }
}

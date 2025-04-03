namespace SLAPScheduling.Application.Queries.MaterialQueries
{
    public class GetMaterialPropertyByIdQuery : IRequest<MaterialPropertyDTO>
    {
        public string MaterialPropertyId { get; set; }

        public GetMaterialPropertyByIdQuery(string materialPropertyId)
        {
            MaterialPropertyId = materialPropertyId;
        }
    }
}

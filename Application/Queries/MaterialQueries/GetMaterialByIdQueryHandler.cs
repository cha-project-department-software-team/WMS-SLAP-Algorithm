namespace SLAPScheduling.Application.Queries.MaterialQueries
{
    public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDTO>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IMapper _mapper;

        public GetMaterialByIdQueryHandler(IMaterialRepository materialRepository, IMapper mapper)
        {
            _materialRepository = materialRepository;
            _mapper = mapper;
        }

        public async Task<MaterialDTO> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
        {
            var material = await _materialRepository.GetByIdAsync(request.MaterialId);
            if (material is null)
            {
                throw new Exception("No result for Materials");
            }

            var materialDTO = _mapper.Map<MaterialDTO>(material);

            return materialDTO;
        }

    }
}

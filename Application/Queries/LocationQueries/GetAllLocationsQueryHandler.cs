using SLAPScheduling.Application.DTOs;

namespace SLAPScheduling.Application.Queries.LocationQueries
{
    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, IEnumerable<LocationDTO>>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public GetAllLocationsQueryHandler(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LocationDTO>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _locationRepository.GetAllLocations();
            if (locations.Count == 0)
            {
                throw new Exception("No locations found");
            }

            var locationDTOs = _mapper.Map<IEnumerable<LocationDTO>>(locations);

            return locationDTOs;


        }


    }
}

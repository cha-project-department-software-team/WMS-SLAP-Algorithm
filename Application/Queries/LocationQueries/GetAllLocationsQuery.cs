using SLAPScheduling.Application.DTOs;

namespace SLAPScheduling.Application.Queries.LocationQueries
{
    public class GetAllLocationsQuery : IRequest<IEnumerable<LocationDTO>>
    {
        public GetAllLocationsQuery()
        {
        }
    }
}

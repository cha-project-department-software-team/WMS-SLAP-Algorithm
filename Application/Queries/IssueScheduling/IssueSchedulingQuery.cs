using SLAPScheduling.Application.DTOs.IssueResults;

namespace SLAPScheduling.Application.Queries.IssueScheduling
{
    public class IssueSchedulingQuery : IRequest<IEnumerable<LocationIDTO>>
    {
        public string WarehouseId { get; set; }

        public IssueSchedulingQuery(string warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}

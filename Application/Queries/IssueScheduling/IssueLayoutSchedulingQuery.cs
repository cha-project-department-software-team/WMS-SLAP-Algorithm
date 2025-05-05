using SLAPScheduling.Application.DTOs.IssueResults;

namespace SLAPScheduling.Application.Queries.IssueScheduling
{
    public class IssueLayoutSchedulingQuery : IRequest<IEnumerable<LocationIDTO>>
    {
        public string WarehouseId { get; set; }

        public IssueLayoutSchedulingQuery(string warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}

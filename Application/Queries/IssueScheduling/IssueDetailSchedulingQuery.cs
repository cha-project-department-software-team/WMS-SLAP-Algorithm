using SLAPScheduling.Application.DTOs.IssueResults;

namespace SLAPScheduling.Application.Queries.IssueScheduling
{
    public class IssueDetailSchedulingQuery : IRequest<IEnumerable<IssueSubLotDetailIDTO>>
    {
        public string WarehouseId { get; set; }

        public IssueDetailSchedulingQuery(string warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}

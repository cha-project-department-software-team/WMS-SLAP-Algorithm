namespace SLAPScheduling.Application.Queries.IssueScheduling
{
    public class IssueSchedulingQuery : IRequest<IEnumerable<IssueSubLotDTO>>
    {
        public string WarehouseId { get; set; }
        public string IssueLotStatus { get; set; }

        public IssueSchedulingQuery(string warehouseId, string issueLotStatus)
        {
            WarehouseId = warehouseId;
            IssueLotStatus = issueLotStatus;
        }
    }
}

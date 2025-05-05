using SLAPScheduling.Application.DTOs.ReceiptResults;

namespace SLAPScheduling.Application.Queries.ReceiptScheduling
{
    public class ReceiptSchedulingQuery : IRequest<IEnumerable<LocationRDTO>>
    {
        public string WarehouseId { get; set; }

        public ReceiptSchedulingQuery(string warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}

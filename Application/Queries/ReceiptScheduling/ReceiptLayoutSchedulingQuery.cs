using SLAPScheduling.Application.DTOs.ReceiptResults;

namespace SLAPScheduling.Application.Queries.ReceiptScheduling
{
    public class ReceiptLayoutSchedulingQuery : IRequest<IEnumerable<LocationRDTO>>
    {
        public string WarehouseId { get; set; }

        public ReceiptLayoutSchedulingQuery(string warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}

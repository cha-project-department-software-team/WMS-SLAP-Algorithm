using SLAPScheduling.Application.DTOs.ReceiptResults;

namespace SLAPScheduling.Application.Queries.ReceiptScheduling
{
    public class ReceiptLayoutSchedulingQuery : IRequest<IEnumerable<LocationRDTO>>
    {
        public string WarehouseId { get; set; }
        public string AlgorithmType { get; set; }

        public ReceiptLayoutSchedulingQuery(string warehouseId, string algorithmType)
        {
            WarehouseId = warehouseId;
            AlgorithmType = algorithmType;
        }
    }
}

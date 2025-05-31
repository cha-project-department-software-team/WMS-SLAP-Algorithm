using SLAPScheduling.Application.DTOs.ReceiptResults;

namespace SLAPScheduling.Application.Queries.ReceiptScheduling
{
    public class ReceiptDetailSchedulingQuery : IRequest<IEnumerable<ReceiptSubLotDetailRDTO>>
    {
        public string WarehouseId { get; set; }
        public string AlgorithmType { get; set; }

        public ReceiptDetailSchedulingQuery(string warehouseId, string algorithmType)
        {
            WarehouseId = warehouseId;
            AlgorithmType = algorithmType;
        }
    }
}

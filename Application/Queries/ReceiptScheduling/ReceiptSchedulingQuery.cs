namespace SLAPScheduling.Application.Queries.ReceiptScheduling
{
    public class ReceiptSchedulingQuery : IRequest<IEnumerable<ReceiptSubLotDTO>>
    {
        public string WarehouseId { get; set; }
        public string ReceiptLotStatus { get; set; }

        public ReceiptSchedulingQuery(string warehouseId, string receiptLotStatus)
        {
            WarehouseId = warehouseId;
            ReceiptLotStatus = receiptLotStatus;
        }
    }
}

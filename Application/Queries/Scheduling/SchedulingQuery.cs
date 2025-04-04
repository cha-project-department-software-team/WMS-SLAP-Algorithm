namespace SLAPScheduling.Application.Queries.Scheduling
{
    public class SchedulingQuery : IRequest<IEnumerable<ReceiptSubLotDTO>>
    {
        public string WarehouseId { get; set; }
        public string ReceiptLotStatus { get; set; }

        public SchedulingQuery(string warehouseId, string receiptLotStatus)
        {
            WarehouseId = warehouseId;
            ReceiptLotStatus = receiptLotStatus;
        }
    }
}

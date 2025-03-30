using SLAPScheduling.Domain.Enum;
using System.Text.Json.Serialization;

namespace SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate
{
    public class InventoryReceipt
    {
        public string InventoryReceiptId { get; set; }
        public DateTime ReceiptDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ReceiptStatus ReceiptStatus { get; set; }
        public string SupplierName { get; set; }
        public string PersonName { get; set; }
        public string WarehouseName { get; set; }
        public List<InventoryReceiptEntry> Entries { get; set; }

        public InventoryReceipt()
        {
        }

        public InventoryReceipt(string inventoryReceiptId, DateTime receiptDate, ReceiptStatus receiptStatus, string supplierName, string personName, string warehouseName, List<InventoryReceiptEntry> entries)
        {
            InventoryReceiptId = inventoryReceiptId;
            ReceiptDate = receiptDate;
            ReceiptStatus = receiptStatus;
            SupplierName = supplierName;
            PersonName = personName;
            WarehouseName = warehouseName;
            Entries = entries;
        }
    }
}

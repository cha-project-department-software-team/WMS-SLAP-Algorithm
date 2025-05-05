namespace SLAPScheduling.Application.DTOs.ReceiptResults
{
    public class ReceiptSubLotRDTO
    {
        public string ReceiptSublotId { get; set; }
        public double ImportedQuantity { get; set; }
        public double StoragePercentage { get; set; }
        public string LocationId { get; set; }
        public string LotNumber { get; set; }

        public ReceiptSubLotRDTO()
        {
        }

        public ReceiptSubLotRDTO(string receiptSublotId, double importedQuantity, double storagePercentage, string locationId, string lotNumber)
        {
            ReceiptSublotId = receiptSublotId;
            ImportedQuantity = importedQuantity;
            StoragePercentage = storagePercentage;
            LocationId = locationId;
            LotNumber = lotNumber;
        }
    }
}

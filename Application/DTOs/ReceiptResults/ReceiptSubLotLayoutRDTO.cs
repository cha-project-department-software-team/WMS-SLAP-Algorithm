namespace SLAPScheduling.Application.DTOs.ReceiptResults
{
    public class ReceiptSubLotLayoutRDTO
    {
        public string ReceiptSublotId { get; set; }
        public double ImportedQuantity { get; set; }
        public double StoragePercentage { get; set; }
        public double StorageLevel { get; set; }
        public string LocationId { get; set; }
        public string LotNumber { get; set; }

        public ReceiptSubLotLayoutRDTO()
        {
        }

        public ReceiptSubLotLayoutRDTO(string receiptSublotId, double importedQuantity, double storagePercentage, double storageLevel, string locationId, string lotNumber)
        {
            ReceiptSublotId = receiptSublotId;
            ImportedQuantity = importedQuantity;
            StoragePercentage = storagePercentage;
            StorageLevel = storageLevel;
            LocationId = locationId;
            LotNumber = lotNumber;
        }
    }
}

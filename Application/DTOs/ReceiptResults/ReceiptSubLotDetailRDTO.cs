namespace SLAPScheduling.Application.DTOs.ReceiptResults
{
    public class ReceiptSubLotDetailRDTO
    {
        public string ReceiptSublotId { get; set; }
        public string MaterialId { get; set; }
        public string MaterialName { get; set; }
        public double ImportedQuantity { get; set; }
        public double StoragePercentage { get; set; }
        public string LocationId { get; set; }
        public string LotNumber { get; set; }

        public ReceiptSubLotDetailRDTO()
        {
        }

        public ReceiptSubLotDetailRDTO(string receiptSublotId, string materialId, string materialName, double importedQuantity, double storagePercentage, string locationId, string lotNumber)
        {
            ReceiptSublotId = receiptSublotId;
            MaterialId = materialId;
            MaterialName = materialName;
            ImportedQuantity = importedQuantity;
            StoragePercentage = storagePercentage;
            LocationId = locationId;
            LotNumber = lotNumber;
        }
    }
}

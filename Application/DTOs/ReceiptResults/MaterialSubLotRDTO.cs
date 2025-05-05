namespace SLAPScheduling.Application.DTOs.ReceiptResults
{
    public class MaterialSubLotRDTO
    {
        public string SubLotId { get; set; }
        public double ExistingQuantity { get; set; }
        public double StoragePercentage { get; set; }
        public string LocationId { get; set; }
        public string LotNumber { get; set; }

        public MaterialSubLotRDTO()
        {
        }

        public MaterialSubLotRDTO(string subLotId, double existingQuantity, double storagePercentage, string locationId, string lotNumber)
        {
            SubLotId = subLotId;
            ExistingQuantity = existingQuantity;
            StoragePercentage = storagePercentage;
            LocationId = locationId;
            LotNumber = lotNumber;
        }
    }
}

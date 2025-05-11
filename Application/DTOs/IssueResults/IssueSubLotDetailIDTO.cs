namespace SLAPScheduling.Application.DTOs.IssueResults
{
    public class IssueSubLotDetailIDTO
    {
        public string IssueSublotId { get; set; }
        public string MaterialId { get; set; }
        public string MaterialName { get; set; }
        public double RequestedQuantity { get; set; }
        public double StoragePercentage { get; set; }
        public string LocationId { get; set; }
        public string MaterialSubLotId { get; set; }
        public string LotNumber { get; set; }

        public IssueSubLotDetailIDTO()
        {
        }

        public IssueSubLotDetailIDTO(string issueSublotId, string materialId, string materialName, double requestedQuantity, double storagePercentage, string locationId, string materialSubLotId, string lotNumber)
        {
            IssueSublotId = issueSublotId;
            MaterialId = materialId;
            MaterialName = materialName;
            RequestedQuantity = requestedQuantity;
            StoragePercentage = storagePercentage;
            LocationId = locationId;
            MaterialSubLotId = materialSubLotId;
            LotNumber = lotNumber;
        }
    }
}

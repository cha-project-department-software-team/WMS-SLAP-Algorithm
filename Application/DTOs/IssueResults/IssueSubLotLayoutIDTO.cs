namespace SLAPScheduling.Application.DTOs.IssueResults
{
    public class IssueSubLotLayoutIDTO
    {
        public string IssueSublotId { get; set; }
        public double RequestedQuantity { get; set; }
        public double StoragePercentage { get; set; }
        public string LocationId { get; set; }
        public string LotNumber { get; set; }

        public IssueSubLotLayoutIDTO()
        {
        }

        public IssueSubLotLayoutIDTO(string issueSublotId, double requestedQuantity, double storagePercentage, string locationId, string lotNumber)
        {
            IssueSublotId = issueSublotId;
            RequestedQuantity = requestedQuantity;
            StoragePercentage = storagePercentage;
            LocationId = locationId;
            LotNumber = lotNumber;
        }
    }
}

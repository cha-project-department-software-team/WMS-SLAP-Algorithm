namespace SLAPScheduling.Application.DTOs.IssueResults
{
    public class IssueSubLotIDTO
    {
        public string IssueSublotId { get; set; }
        public double RequestedQuantity { get; set; }
        public double StoragePercentage { get; set; }
        public string LocationId { get; set; }
        public string LotNumber { get; set; }

        public IssueSubLotIDTO()
        {
        }

        public IssueSubLotIDTO(string issueSublotId, double requestedQuantity, double storagePercentage, string locationId, string lotNumber)
        {
            IssueSublotId = issueSublotId;
            RequestedQuantity = requestedQuantity;
            StoragePercentage = storagePercentage;
            LocationId = locationId;
            LotNumber = lotNumber;
        }
    }
}

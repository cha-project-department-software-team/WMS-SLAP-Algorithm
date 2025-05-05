using SLAPScheduling.Application.DTOs.ReceiptResults;

namespace SLAPScheduling.Application.DTOs.IssueResults
{
    public class LocationIDTO
    {
        public string LocationId { get; set; }
        public List<MaterialSubLotIDTO> MaterialSubLots { get; set; }
        public List<IssueSubLotIDTO> IssueSubLots { get; set; }

        public LocationIDTO()
        {
        }

        public LocationIDTO(string locationId, List<MaterialSubLotIDTO> materialSubLots, List<IssueSubLotIDTO> issueSubLots)
        {
            LocationId = locationId;
            MaterialSubLots = materialSubLots;
            IssueSubLots = issueSubLots;
        }
    }
}

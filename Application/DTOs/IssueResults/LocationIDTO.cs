using SLAPScheduling.Application.DTOs.ReceiptResults;

namespace SLAPScheduling.Application.DTOs.IssueResults
{
    public class LocationIDTO
    {
        public string LocationId { get; set; }
        public List<MaterialSubLotIDTO> MaterialSubLots { get; set; }
        public List<IssueSubLotLayoutIDTO> IssueSubLots { get; set; }

        public LocationIDTO()
        {
        }

        public LocationIDTO(string locationId, List<MaterialSubLotIDTO> materialSubLots, List<IssueSubLotLayoutIDTO> issueSubLots)
        {
            LocationId = locationId;
            MaterialSubLots = materialSubLots;
            IssueSubLots = issueSubLots;
        }
    }
}

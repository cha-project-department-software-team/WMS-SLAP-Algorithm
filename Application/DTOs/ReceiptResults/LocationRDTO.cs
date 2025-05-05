namespace SLAPScheduling.Application.DTOs.ReceiptResults
{
    public class LocationRDTO
    {
        public string LocationId { get; set; }
        public List<MaterialSubLotRDTO> MaterialSubLots { get; set; }
        public List<ReceiptSubLotLayoutRDTO> ReceiptSubLots { get; set; }

        public LocationRDTO()
        {
        }

        public LocationRDTO(string locationId, List<MaterialSubLotRDTO> materialSubLots, List<ReceiptSubLotLayoutRDTO> receiptSubLots)
        {
            LocationId = locationId;
            MaterialSubLots = materialSubLots;
            ReceiptSubLots = receiptSubLots;
        }
    }
}

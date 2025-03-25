using SLAP.AggregateModels.InventoryIssueAggregate;
using SLAP.AggregateModels.StorageAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialSubLot
    {
        public string subLotId { get; set; }
        public LotStatus subLotStatus { get; set; }
        public double existingQuality { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }
        public string locationId { get; set; }
        public Location  location{ get; set; }
        public string lotNumber { get; set; }
        public MaterialLot materialLot { get; set; }
        public List<IssueSublot> issueSublots { get; set; }

        public MaterialSubLot(string subLotId, LotStatus subLotStatus, double existingQuality, UnitOfMeasure unitOfMeasure, string locationId, string lotNumber)
        {
            this.subLotId = subLotId;
            this.subLotStatus = subLotStatus;
            this.existingQuality = existingQuality;
            this.unitOfMeasure = unitOfMeasure;
            this.locationId = locationId;
            this.lotNumber = lotNumber;
        }
    }
}

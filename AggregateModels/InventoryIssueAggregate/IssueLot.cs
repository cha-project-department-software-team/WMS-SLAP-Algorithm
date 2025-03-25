using SLAP.AggregateModels.MaterialAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.InventoryIssueAggregate
{
    public class IssueLot
    {
        public string issueLotId { get; set; }
        public double requestedQuantity { get; set; }
        public MaterialLot materialLot { get; set; }
        public List<IssueSublot> issueSublots { get; set; }
        public LotStatus issueLotStatus { get; set; }

        public IssueLot()
        {
        }

        public IssueLot(string issueLotId, double requestedQuantity, MaterialLot materialLot, List<IssueSublot> issueSublots, LotStatus issueLotStatus)
        {
            this.issueLotId = issueLotId;
            this.requestedQuantity = requestedQuantity;
            this.materialLot = materialLot;
            this.issueSublots = issueSublots;
            this.issueLotStatus = issueLotStatus;
        }
    }
}

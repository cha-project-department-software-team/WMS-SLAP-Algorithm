using SLAP.AggregateModels.MaterialAggregate;

namespace SLAP.AggregateModels.InventoryIssueAggregate
{
    public class IssueSublot
    {
        public string issueSublotId { get; set; }
        public double requestedQuantity { get; set; }
        public string sublotId { get; set; }
        public MaterialSubLot materialSublot { get; set; }
        public string issueLotId { get; set; }
        public IssueLot issueLot { get; set; }

        public IssueSublot()
        {
        }

        public IssueSublot(string issueSublotId, double requestedQuantity, string materialSublotId, string issueLotId)
        {
            this.issueSublotId = issueSublotId;
            this.requestedQuantity = requestedQuantity;
            this.sublotId = materialSublotId;
            this.issueLotId = issueLotId;
        }
    }
}

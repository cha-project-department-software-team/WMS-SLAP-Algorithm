using SLAP.AggregateModels.MaterialAggregate;

namespace SLAP.AggregateModels.InventoryIssueAggregate
{
    public class IssueSublot
    {
        public string issueSublotId { get; set; }
        public double requestedQuantity { get; set; }
        public MaterialSubLot materialSublot { get; set; }

        public IssueSublot()
        {
        }

        public IssueSublot(string issueSublotId, double requestedQuantity, MaterialSubLot materialSublot)
        {
            this.issueSublotId = issueSublotId;
            this.requestedQuantity = requestedQuantity;
            this.materialSublot = materialSublot;
        }
    }
}

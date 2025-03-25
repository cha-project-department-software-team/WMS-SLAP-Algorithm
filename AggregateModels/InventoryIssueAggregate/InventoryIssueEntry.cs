using SLAP.AggregateModels.MaterialAggregate;

namespace SLAP.AggregateModels.InventoryIssueAggregate
{
    public class InventoryIssueEntry 
    {
        public string inventoryIssueEntryId { get; set; }

        public string purchaseOrderNumber { get; set; }
        public double requestedQuantity { get; set; }
        public string note { get; set; }
        public string materialId { get; set; }
        public Material material { get; set; }
        public string issueLotId { get; set; }
        public IssueLot issueLot { get; set; }
        public string inventoryIssueId { get; set; }
        public InventoryIssue inventoryIssue { get; set; }

        public InventoryIssueEntry(string inventoryIssueEntryId, string purchaseOrderNumber, double requestedQuantity, string note, string materialId, string issueLotId, string inventoryIssueId)
        {
            this.inventoryIssueEntryId = inventoryIssueEntryId;
            this.purchaseOrderNumber = purchaseOrderNumber;
            this.requestedQuantity = requestedQuantity;
            this.note = note;
            this.materialId = materialId;
            this.issueLotId = issueLotId;
            this.inventoryIssueId = inventoryIssueId;
        }
    }
}

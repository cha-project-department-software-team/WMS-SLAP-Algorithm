using SLAP.AggregateModels.MaterialAggregate;

namespace SLAP.AggregateModels.InventoryIssueAggregate
{
    public class InventoryIssueEntry 
    {
        public string inventoryIssueEntryId { get; set; }
        public string purchaseOrderNumber { get; set; }
        public double requestedQuantity { get; set; }
        public Material material { get; set; }
        public IssueLot issueLot { get; set; }
        public string note { get; set; }

        public InventoryIssueEntry()
        {
        }

        public InventoryIssueEntry(string inventoryIssueEntryId, string purchaseOrderNumber, double requestedQuantity, Material material, IssueLot issueLot, string note)
        {
            this.inventoryIssueEntryId = inventoryIssueEntryId;
            this.purchaseOrderNumber = purchaseOrderNumber;
            this.requestedQuantity = requestedQuantity;
            this.material = material;
            this.issueLot = issueLot;
            this.note = note;
        }
    }
}

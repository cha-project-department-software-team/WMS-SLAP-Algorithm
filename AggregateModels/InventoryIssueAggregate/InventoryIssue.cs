using SLAP.AggregateModels.PartyAggregate;
using SLAP.AggregateModels.StorageAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.InventoryIssueAggregate
{
    public class InventoryIssue
    {
        public string inventoryIssueId { get; set; }

        public DateTime issueDate { get; set; }
        public IssueStatus issueStatus { get; set; }

        public string customerId { get; set; }
        public Customer customer { get; set; }

        public string personId { get; set; }
        public Person issuedBy { get; set; }

        public string warehouseId { get; set; }
        public Warehouse warehouse { get; set; }

        public List<InventoryIssueEntry> entries { get; set; }

        public InventoryIssue()
        {
        }

        public InventoryIssue(string inventoryIssueId, DateTime issueDate, IssueStatus issueStatus, string customerId, string personId, string warehouseId)
        {
            this.inventoryIssueId = inventoryIssueId;
            this.issueDate = issueDate;
            this.issueStatus = issueStatus;
            this.customerId = customerId;
            this.personId = personId;
            this.warehouseId = warehouseId;
            this.entries = new List<InventoryIssueEntry>();
        }
    }
}

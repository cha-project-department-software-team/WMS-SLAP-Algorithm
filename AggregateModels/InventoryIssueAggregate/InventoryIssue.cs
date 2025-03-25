using SLAP.AggregateModels.PartyAggregate;
using SLAP.AggregateModels.StorageAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.InventoryIssueAggregate
{
    public class InventoryIssue
    {
        public string inventoryIssueId { get; set; }
        public Customer customer { get; set; }
        public Person person { get; set; }
        public Warehouse warehouse { get; set; }
        public List<InventoryIssueEntry> entries { get; set; }
        public DateTime issueDate { get; set; }
        public IssueStatus issueStatus { get; set; }

        public InventoryIssue()
        {
        }

        public InventoryIssue(string inventoryIssueId, Customer customer, Person person, Warehouse warehouse, List<InventoryIssueEntry> entries, DateTime issueDate, IssueStatus issueStatus)
        {
            this.inventoryIssueId = inventoryIssueId;
            this.customer = customer;
            this.person = person;
            this.warehouse = warehouse;
            this.entries = entries;
            this.issueDate = issueDate;
            this.issueStatus = issueStatus;
        }
    }
}

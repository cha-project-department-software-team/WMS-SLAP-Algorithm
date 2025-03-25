using SLAP.AggregateModels.InventoryIssueAggregate;
using SLAP.AggregateModels.InventoryReceiptAggregate;
using SLAP.AggregateModels.PartyAggregate.People;
using SLAP.Enum;

namespace SLAP.AggregateModels.PartyAggregate
{
    public class Person
    {
        public string personId { get; set; }
        public string personName { get; set; }
        public EmployeeType role { get; set; }
        public List<InventoryReceipt> inventoryReceipts { get; set; }
        public List<InventoryIssue> inventoryIssues { get; set; }
        public List<PersonProperty> properties { get; set; }

        public Person(string personId, string personName, EmployeeType role)
        {
            this.personId = personId;
            this.personName = personName;
            this.role = role;
        }
    }
}

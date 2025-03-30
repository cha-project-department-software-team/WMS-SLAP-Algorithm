using SLAPScheduling.Domain.AggregateModels.InventoryIssueAggregate;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations;

namespace SLAPScheduling.Domain.AggregateModels.PartyAggregate.Customers
{
    public class Customer : Entity, IAggregateRoot
    {
        [Key]
        public string customerId { get; set; }

        public string customerName { get; set; }
        public string address { get; set; }
        public string contactDetails { get; set; }
        public List<InventoryIssue> inventoryIssues { get; set; }

        public Customer(string customerId, string customerName, string address, string contactDetails)
        {
            this.customerId = customerId;
            this.customerName = customerName;
            this.address = address;
            this.contactDetails = contactDetails;
        }
    }
}

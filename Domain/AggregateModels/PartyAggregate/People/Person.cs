﻿namespace SLAPScheduling.Domain.AggregateModels.PartyAggregate.People
{
    public class Person : Entity, IAggregateRoot
    {
        [Key]
        public string personId { get; set; }

        public string personName { get; set; }
        public EmployeeType role { get; set; }

        public List<InventoryReceipt> inventoryReceipts { get; set; }
        public List<InventoryIssue> inventoryIssues { get; set; }
        public List<MaterialLotAdjustment> materialLotAdjustments { get; set; }
        public List<PersonProperty> properties { get; set; }

        public Person(string personId, string personName, EmployeeType role)
        {
            this.personId = personId;
            this.personName = personName;
            this.role = role;
        }
    }
}

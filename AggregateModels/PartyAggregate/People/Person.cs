using SLAPScheduling.AggregateModels.Properties;

namespace SLAP.AggregateModels.PartyAggregate
{
    public class Person
    {
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public string EmployeeType { get; set; }
        public List<Property> personProperties { get; set; }

        public Person()
        {

        }

        public Person(string personId, string personName, string employeeType, List<Property> personProperties)
        {
            PersonId = personId;
            PersonName = personName;
            EmployeeType = employeeType;
            this.personProperties = personProperties;
        }
    }
}

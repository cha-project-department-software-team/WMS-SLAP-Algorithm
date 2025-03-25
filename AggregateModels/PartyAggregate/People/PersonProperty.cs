using SLAP.Enum;

namespace SLAP.AggregateModels.PartyAggregate.People
{
    public class PersonProperty
    {
        public string propertyId { get; set; }
        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        public PersonProperty(string propertyId, string propertyName, string propertyValue, UnitOfMeasure unitOfMeasure)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.unitOfMeasure = unitOfMeasure;
        }
    }
}

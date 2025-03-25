using SLAP.Enum;

namespace SLAP.AggregateModels.MaterialAggregate
{ 
    public class MaterialClassProperty
    {
        public string propertyId { get; set; }
        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        public MaterialClassProperty(string propertyId, string propertyName, string propertyValue, UnitOfMeasure unitOfMeasure)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.unitOfMeasure = unitOfMeasure;
        }
    }
}

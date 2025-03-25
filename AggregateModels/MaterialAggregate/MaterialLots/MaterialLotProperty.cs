using SLAP.Enum;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialLotProperty
    {
        public string propertyId { get; set; }
        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public string lotNumber { get; set; }
        public MaterialLot materialLot { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        public MaterialLotProperty(string propertyId, string propertyName, string propertyValue, string lotNumber, UnitOfMeasure unitOfMeasure)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.lotNumber = lotNumber;
            this.unitOfMeasure = unitOfMeasure;
        }
    }
}

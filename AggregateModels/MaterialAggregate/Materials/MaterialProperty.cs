using SLAP.Enum;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialProperty
    {
        public string propertyId { get; set; }
        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }
        public string materialId { get; set; }
        public Material material { get; set; }

        public MaterialProperty(string propertyId, string propertyName, string propertyValue, UnitOfMeasure unitOfMeasure, string materialId)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.unitOfMeasure = unitOfMeasure;
            this.materialId = materialId;
        }
    }
}

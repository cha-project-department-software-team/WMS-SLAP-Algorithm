namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialClass
    {
        public string materialClassId { get; set; }
        public string className { get; set; }
        public List<MaterialClassProperty> properties { get; set; }

        public MaterialClass(string materialClassId, string className, List<MaterialClassProperty> properties) : this(materialClassId, className)
        {
            this.properties = properties;
        }

        public MaterialClass(string materialClassId, string className)
        {
            this.materialClassId = materialClassId;
            this.className = className;
            this.properties = new List<MaterialClassProperty>();
        }
    }
}
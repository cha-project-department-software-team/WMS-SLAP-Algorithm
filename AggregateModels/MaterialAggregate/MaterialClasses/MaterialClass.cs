using SLAPScheduling.AggregateModels.Properties;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialClass
    {
        public string MaterialClassId { get; set; }
        public string ClassName { get; set; }
        public List<Property> Properties { get; set; }
        public List<Material> Materials { get; set; }

        public MaterialClass()
        {
        }

        public MaterialClass(string materialClassId, string className, List<Property> properties, List<Material> materials)
        {
            MaterialClassId = materialClassId;
            ClassName = className;
            Properties = properties;
            Materials = materials;
        }
    }
}
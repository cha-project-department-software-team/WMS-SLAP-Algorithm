namespace SLAP.AggregateModels.MaterialAggregate
{
    public class Material
    {
        public string materialId { get; set; }
        public string materialName { get; set; }
        public string materialClassId { get; set; }
        public MaterialClass materialClass { get; set; }
        public List<MaterialProperty> properties { get; set; }

        public Material(string materialId, string materialName, string materialClassId)
        {
            this.materialId = materialId;
            this.materialName = materialName;
            this.materialClassId = materialClassId;
            this.properties = new List<MaterialProperty>();
        }
    }
}

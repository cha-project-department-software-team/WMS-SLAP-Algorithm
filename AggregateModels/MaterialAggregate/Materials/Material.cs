using SLAP.AggregateModels.InventoryIssueAggregate;
using SLAP.AggregateModels.InventoryReceiptAggregate;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class Material
    {
        public string materialId { get; set; }
        public string materialName { get; set; }
        public string materialClassId { get; set; }
        public MaterialClass materialClass { get; set; }
        public List<MaterialProperty> properties { get; set; }
        public List<MaterialLot> lots { get; set; }
        public List<InventoryReceiptEntry> receiptEntries  { get; set; }
        public List<InventoryIssueEntry> issueEntries  { get; set; }

        public Material(string materialId, string materialName, string materialClassId)
        {
            this.materialId = materialId;
            this.materialName = materialName;
            this.materialClassId = materialClassId;
            this.properties = new List<MaterialProperty>();
        }
    }
}

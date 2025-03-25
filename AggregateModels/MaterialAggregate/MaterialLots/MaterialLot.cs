using SLAP.AggregateModels.InventoryIssueAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialLot
    {
        public string lotNumber { get; set; }
        public double existingQuantity { get; set; }
        public Material material { get; set; }
        public List<MaterialSubLot> subLots{ get; set; }
        public LotStatus lotStatus { get; set; }

        public MaterialLot()
        {
        }

        public MaterialLot(string lotNumber, double existingQuantity, Material material, List<MaterialSubLot> subLots, LotStatus lotStatus)
        {
            this.lotNumber = lotNumber;
            this.existingQuantity = existingQuantity;
            this.material = material;
            this.subLots = subLots;
            this.lotStatus = lotStatus;
        }
    }
}

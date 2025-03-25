using SLAP.AggregateModels.InventoryIssueAggregate;
using SLAP.Enum;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialLot
    {
        
        public string lotNumber { get; set; }
        public LotStatus lotStatus { get; set; }
        public string materialId { get; set; }
        public Material material { get; set; }
        public double exisitingQuantity { get; set; }
        public List<IssueLot> issueLots { get; set; }
        public List<MaterialLotProperty> properties { get; set; }
        public List<MaterialSubLot> subLots{ get; set; }

        public MaterialLot(string lotNumber, LotStatus lotStatus, string materialId, double exisitingQuantity)
        {
            this.lotNumber = lotNumber;
            this.lotStatus = lotStatus;
            this.materialId = materialId;
            this.exisitingQuantity = exisitingQuantity;
            this.subLots = new List<MaterialSubLot>();
            this.properties = new List<MaterialLotProperty>();
        }
    }
}

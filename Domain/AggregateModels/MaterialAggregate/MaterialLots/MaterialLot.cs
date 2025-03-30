using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialSubLots;
using SLAPScheduling.Domain.AggregateModels.Properties;
using SLAPScheduling.Domain.Enum;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using SLAPScheduling.Domain.AggregateModels.InventoryIssueAggregate;
using SLAPScheduling.Domain.AggregateModels.InventoryLogAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialLotAdjustmentAggregate;

namespace SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialLots
{
    public class MaterialLot : Entity, IAggregateRoot
    {
        [Key]
        public string lotNumber { get; set; }

        public LotStatus lotStatus { get; set; }

        [ForeignKey("materialId")]
        public string materialId { get; set; }
        public Material material { get; set; }
        public double existingQuantity { get; set; }
        public List<IssueLot> issueLots { get; set; }
        public List<MaterialLotProperty> properties { get; set; }
        public List<MaterialSubLot> subLots { get; set; }
        public List<InventoryLog> inventoryLogs { get; set; }
        public List<MaterialLotAdjustment> materialLotAdjustments { get; set; }

        public MaterialLot(string lotNumber, LotStatus lotStatus, string materialId, double existingQuantity)
        {
            this.lotNumber = lotNumber;
            this.lotStatus = lotStatus;
            this.materialId = materialId;
            this.existingQuantity = existingQuantity;
            this.subLots = new List<MaterialSubLot>();
            this.properties = new List<MaterialLotProperty>();
        }
    }
}

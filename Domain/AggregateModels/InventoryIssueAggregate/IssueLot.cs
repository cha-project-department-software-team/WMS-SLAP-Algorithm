using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialLots;
using SLAPScheduling.Domain.Enum;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAPScheduling.Domain.AggregateModels.InventoryIssueAggregate
{
    public class IssueLot : Entity, IAggregateRoot
    {
        [Key]
        public string issueLotId { get; set; }

        public double requestedQuantity { get; set; }
        public List<IssueSublot> issueSublots { get; set; }
        public LotStatus issueLotStatus { get; set; }

        [ForeignKey("materialId")]
        public string materialLotId { get; set; } 
        public MaterialLot materialLot { get; set; }

        [ForeignKey("inventoryIssueEntryId")]
        public string inventoryIssueEntryId { get; set; }
        public InventoryIssueEntry inventoryIssueEntry { get; set; }

        public IssueLot(string issueLotId, double requestedQuantity, LotStatus issueLotStatus, string materialLotId, string inventoryIssueEntryId)
        {
            this.issueLotId = issueLotId;
            this.requestedQuantity = requestedQuantity;
            this.issueLotStatus = issueLotStatus;
            this.materialLotId = materialLotId;
            this.inventoryIssueEntryId = inventoryIssueEntryId;
            this.issueSublots = new List<IssueSublot>();
        }
    }
}

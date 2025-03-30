using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialLots;
using SLAPScheduling.Domain.AggregateModels.PartyAggregate.People;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;
using SLAPScheduling.Domain.Enum;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAPScheduling.Domain.AggregateModels.MaterialLotAdjustmentAggregate
{
    public class MaterialLotAdjustment : Entity, IAggregateRoot 
    {
        [Key]
        public string materialLotAdjustmentId { get; set; }

        public DateTime adjustmentDate { get; set; }
        public double previousQuantity { get; set; }
        public double adjustedQuantity { get; set; }
        public AdjustmentReason reason { get; set; }
        public AdjustmentStatus status { get; set; }
        public string note { get; set; }

        [ForeignKey("lotNumber")]
        public string lotNumber { get; set; }
        public MaterialLot materialLot { get; set; }

        [ForeignKey("warehouseId")]
        public string warehouseId { get; set; }
        public Warehouse warehouse { get; set; }

        [ForeignKey("personId")]
        public string personId { get; set; }
        public Person adjustedBy { get; set; }

        public MaterialLotAdjustment(string materialLotAdjustmentId, double previousQuantity, double adjustedQuantity, AdjustmentReason reason, AdjustmentStatus status, string note, string lotNumber, string warehouseId, string personId)
        {
            this.materialLotAdjustmentId = materialLotAdjustmentId;
            adjustmentDate = GetVietnamTime();
            this.previousQuantity = previousQuantity;
            this.adjustedQuantity = adjustedQuantity;
            this.reason = reason;
            this.status = status;
            this.note = note;
            this.lotNumber = lotNumber;
            this.warehouseId = warehouseId;
            this.personId = personId;
        }

        private static DateTime GetVietnamTime()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

        }
    }
}

using SLAPScheduling.Domain.AggregateModels.InventoryIssueAggregate;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialLots;
using SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations;
using SLAPScheduling.Domain.Enum;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialSubLots
{
    public class MaterialSubLot : Entity, IAggregateRoot
    {
        [Key]
        public string subLotId { get; set; }
        public LotStatus subLotStatus { get; set; }
        public double existingQuantity { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        [ForeignKey("locationId")]
        public string locationId { get; set; }
        public Location location { get; set; }

        [ForeignKey("lotNumber")]
        public string lotNumber { get; set; }
        public MaterialLot materialLot { get; set; }
        public List<IssueSublot> issueSublots { get; set; }

        public MaterialSubLot(string subLotId, LotStatus subLotStatus, double existingQuantity, UnitOfMeasure unitOfMeasure, string locationId, string lotNumber)
        {
            this.subLotId = subLotId;
            this.subLotStatus = subLotStatus;
            this.existingQuantity = existingQuantity;
            this.unitOfMeasure = unitOfMeasure;
            this.locationId = locationId;
            this.lotNumber = lotNumber;
        }

        #region Retrieve Methods

        /// <summary>
        /// Retrieve the Material in the Material Lot.
        /// </summary>
        /// <returns></returns>
        public Material? GetMaterial()
        {
            return this.materialLot?.material;
        }

        #endregion
    }
}

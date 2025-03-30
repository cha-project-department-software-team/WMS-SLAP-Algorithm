using SLAPScheduling.Domain.Enum;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials
{
    public class MaterialProperty : Entity, IAggregateRoot
    {
        [Key]
        public string propertyId { get; set; }
        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        [ForeignKey("materialId")]
        public string materialId { get; set; }
        public Material material { get; set; }

        public MaterialProperty(string propertyId, string propertyName, string propertyValue, UnitOfMeasure unitOfMeasure, string materialId)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.unitOfMeasure = unitOfMeasure;
            this.materialId = materialId;
        }
    }
}

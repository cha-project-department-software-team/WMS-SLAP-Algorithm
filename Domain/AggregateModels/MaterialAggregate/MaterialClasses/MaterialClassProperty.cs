using SLAPScheduling.Domain.Enum;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialClasses
{
    public class MaterialClassProperty : Entity, IAggregateRoot
    {
        [Key]
        public string propertyId { get; set; }
        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        [ForeignKey("materialClassId")]
        public string materialClassId { get; set; }
        public MaterialClass materialClass { get; set; }

        public MaterialClassProperty(string propertyId, string propertyName, string propertyValue, UnitOfMeasure unitOfMeasure, string materialClassId)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.unitOfMeasure = unitOfMeasure;
            this.materialClassId = materialClassId;
        }
    }
}

using SLAPScheduling.Domain.Enum;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SLAPScheduling.Domain.AggregateModels.StorageAggregate.Locations
{
    public class LocationProperty : IAggregateRoot
    {
        [Key]
        public string propertyId { get; set; }

        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        [ForeignKey("equipmentId")]
        public string locationId { get; set; }
        public Location location { get; set; }

        public LocationProperty(string propertyId, string propertyName, string propertyValue, UnitOfMeasure unitOfMeasure, string locationId)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.unitOfMeasure = unitOfMeasure;
            this.locationId = locationId;
        }
    }
}

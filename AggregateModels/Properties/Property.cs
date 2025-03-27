using SLAP.Enum;
using System.Text.Json.Serialization;

namespace SLAPScheduling.AggregateModels.Properties
{
    public class Property
    {
        public string PropertyId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public Property(string propertyId, string propertyName, string propertyValue, UnitOfMeasure unitOfMeasure)
        {
            PropertyId = propertyId;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            UnitOfMeasure = unitOfMeasure;
        }
    }
}

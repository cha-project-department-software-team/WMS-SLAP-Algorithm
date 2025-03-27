using SLAP.Enum;
using SLAPScheduling.AggregateModels.Properties;
using System.Text.Json.Serialization;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialLot
    {
        public string LotNumber { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LotStatus LotStatus { get; set; }
        public string MaterialId { get; set; }
        public double ExisitingQuantity { get; set; }
        public List<Property> Properties { get; set; }
        public List<MaterialSubLot> SubLots { get; set; }
        public MaterialLot()
        {
        }

        public MaterialLot(string lotNumber, LotStatus lotStatus, string materialId, double exisitingQuantity, List<Property> properties, List<MaterialSubLot> subLots)
        {
            LotNumber = lotNumber;
            LotStatus = lotStatus;
            MaterialId = materialId;
            ExisitingQuantity = exisitingQuantity;
            Properties = properties;
            SubLots = subLots;
        }
    }
}

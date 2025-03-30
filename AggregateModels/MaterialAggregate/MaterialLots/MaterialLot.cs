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
        public Material Material { get; set; }
        public double ExistingQuantity { get; set; }
        public List<Property> Properties { get; set; }
        public List<MaterialSubLot> SubLots { get; set; }

        #region Constructors
        public MaterialLot()
        {
        }

        public MaterialLot(string lotNumber, LotStatus lotStatus, string materialId, double existingQuantity, List<Property> properties, List<MaterialSubLot> subLots)
        {
            LotNumber = lotNumber;
            LotStatus = lotStatus;
            MaterialId = materialId;
            ExistingQuantity = existingQuantity;
            Properties = properties;
            SubLots = subLots;
        }

        #endregion
    }
}

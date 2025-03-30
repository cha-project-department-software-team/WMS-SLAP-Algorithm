using SLAP.AggregateModels.StorageAggregate;
using SLAP.Enum;
using System.Text.Json.Serialization;

namespace SLAP.AggregateModels.MaterialAggregate
{
    public class MaterialSubLot
    {
        public string SubLotId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LotStatus SubLotStatus { get; set; }
        public double ExistingQuality { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UnitOfMeasure UnitOfMeasure { get; set; }
        public string LocationId { get; set; }
        public Location Location { get; set; }
        public string LotNumber { get; set; }
        public MaterialLot MaterialLot { get; set; }

        #region Constructor
        public MaterialSubLot(string subLotId, LotStatus subLotStatus, double existingQuality, UnitOfMeasure unitOfMeasure, string locationId, string lotNumber)
        {
            SubLotId = subLotId;
            SubLotStatus = subLotStatus;
            ExistingQuality = existingQuality;
            UnitOfMeasure = unitOfMeasure;
            LocationId = locationId;
            LotNumber = lotNumber;
        }

        #endregion

        #region Retrieve Methods

        /// <summary>
        /// Retrieve the Material in the Material Lot.
        /// </summary>
        /// <returns></returns>
        public Material? GetMaterial()
        {
            return this.MaterialLot?.Material;
        }

        #endregion
    }
}

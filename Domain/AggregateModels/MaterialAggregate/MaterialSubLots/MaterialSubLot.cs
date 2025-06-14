﻿namespace SLAPScheduling.Domain.AggregateModels.MaterialAggregate.MaterialSubLots
{
    public class MaterialSubLot : Entity, IAggregateRoot
    {
        [Key]
        public string subLotId { get; set; }
        public LotStatus subLotStatus { get; set; }
        public double existingQuality { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        [ForeignKey("locationId")]
        public string locationId { get; set; }
        public Location location { get; set; }

        [ForeignKey("lotNumber")]
        public string lotNumber { get; set; }
        public MaterialLot materialLot { get; set; }

        public List<IssueSublot> issueSublots { get; set; }

        public MaterialSubLot(string subLotId, LotStatus subLotStatus, double existingQuality, UnitOfMeasure unitOfMeasure, string locationId, string lotNumber)
        {
            this.subLotId = subLotId;
            this.subLotStatus = subLotStatus;
            this.existingQuality = existingQuality;
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

        public double GetSublotVolume()
        {
            if (this.existingQuality == 0)
                return 0;

            var material = this.GetMaterial();
            if (material is not null)
            {
                var packetSize = material.GetPacketSize();
                var sublotPacketQuantity = Math.Ceiling(this.existingQuality / packetSize);

                return material.GetPacketVolume() * sublotPacketQuantity;
            }

            return 0;
        }

        public double GetStoragePercentage(Location location)
        {
            var locationVolume = location.GetLocationVolume();
            return locationVolume > 0 ? this.GetSublotVolume() / locationVolume : 0;
        }

        #endregion
    }
}

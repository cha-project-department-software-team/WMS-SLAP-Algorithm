namespace SLAPScheduling.Domain.AggregateModels.InventoryIssueAggregate
{
    public class IssueSublot : Entity, IAggregateRoot
    {
        [Key]
        public string issueSublotId { get; set; }

        public double requestedQuantity { get; set; }

        [ForeignKey("materialSublotId")]
        public string sublotId { get; set; }
        public MaterialSubLot materialSublot { get; set; }

        [ForeignKey("issueLotId")]
        public string issueLotId { get; set; }
        public IssueLot issueLot { get; set; }

        public IssueSublot()
        {
        }

        public IssueSublot(string issueSublotId, double requestedQuantity, string materialSublotId, string issueLotId)
        {
            this.issueSublotId = issueSublotId;
            this.requestedQuantity = requestedQuantity;
            this.sublotId = materialSublotId;
            this.issueLotId = issueLotId;
        }

        #region Update Methods
        public void UpdateMaterialSublot(MaterialSubLot materialSublot)
        {
            this.materialSublot = materialSublot;
        }

        public void UpdateIssueLot(IssueLot issueLot)
        {
            this.issueLot = issueLot;
        }

        #endregion

        #region Retrieve Methods

        public Material? GetMaterial()
        {
            if (this.issueLot is not null)
            {
                return this.issueLot.material;
            }

            return null;
        }

        public string GetLocationId()
        {
            if (this.materialSublot is not null)
            {
                return this.materialSublot.locationId;
            }

            return string.Empty;
        }

        public Location? GetLocation()
        {
            if (this.materialSublot is not null)
            {
                return this.materialSublot.location;
            }

            return null;
        }

        public double GetStoragePercentage()
        {
            var material = this.GetMaterial();
            var location = this.GetLocation();
            if (material is not null && location is not null)
            {
                var packetQuantity = material.GetPacketSize() > 0 ? this.requestedQuantity / material.GetPacketSize() : 0;
                var subLotVolume = material is not null ? material.GetPacketVolume() * packetQuantity : 0;

                var locationVolume = location.GetLocationVolume();
                return locationVolume > 0 ? subLotVolume / locationVolume : 0;
            }

            return 0;
        }

        #endregion
    }
}

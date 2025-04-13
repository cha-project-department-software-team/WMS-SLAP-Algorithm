namespace SLAPScheduling.Algorithm.Helpers
{
    public class ReceiptLotSplitter : IDisposable
    {
        private List<ReceiptLot> receiptLots { get; set; }
        private double locationVolume { get; set; }

        #region Constructor

        public ReceiptLotSplitter(List<ReceiptLot> receiptLots, Warehouse warehouse)
        {
            this.receiptLots = receiptLots;
            this.locationVolume = warehouse?.locations?.Count > 0 ? warehouse.GetLocationVolume() : 0;
        }

        #endregion

        #region Split Receipt Lots to multiple Receipt Sublots

        /// <summary>
        /// Split receipt entries to multiple sublots (with empty location) based on the volume size.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ReceiptSublot> GetReceiptSubLots()
        {
            foreach (var receiptLot in this.receiptLots)
            {
                var material = receiptLot.material;
                if (material is not null)
                {
                    int packetQuantityPerLocation = CalculatePacketQuantityPerLocation(material);
                    var packetSize = material.GetPacketSize();
                    int subLotCount = CalculateNumberOfSubLot(receiptLot.importedQuantity, packetSize, packetQuantityPerLocation);

                    for (int subLotIndex = 0; subLotIndex < subLotCount; subLotIndex++)
                    {
                        var subLotId = $"{receiptLot.receiptLotId}_{subLotIndex}";
                        var quantityPerLocation = packetSize * packetQuantityPerLocation;
                        var subLotQuantity = subLotIndex == subLotCount - 1 ? receiptLot.importedQuantity % quantityPerLocation : quantityPerLocation;
                        if (subLotQuantity > 0)
                        {
                            var receiptSubLot = new ReceiptSublot(receiptSublotId: subLotId,
                                                              importedQuantity: subLotQuantity,
                                                              subLotStatus: LotStatus.Pending,
                                                              unitOfMeasure: UnitOfMeasure.None,
                                                              locationId: string.Empty,
                                                              receiptLotId: receiptLot.receiptLotId);

                            receiptSubLot.UpdateReceiptLot(receiptLot);
                            yield return receiptSubLot;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the locations which 
        /// </summary>
        /// <param name="lotQuantity"></param>
        /// <param name="packetQuantityPerLocation"></param>
        /// <returns></returns>
        private int CalculateNumberOfSubLot(double lotQuantity, int packetSize, int packetQuantityPerLocation)
        {
            var lotPacketQuantity = Math.Ceiling(lotQuantity / packetSize);

            return packetQuantityPerLocation > 0 ? (int)Math.Ceiling(lotPacketQuantity / packetQuantityPerLocation) : 0;
        }

        /// <summary>
        /// Calculate the quantity of material per location.
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        private int CalculatePacketQuantityPerLocation(Material? material)
        {
            if (material is null)
                return 0;

            var materialPacketVolume = material.GetPacketVolume();
            return locationVolume > 0 && materialPacketVolume > 0 ? (int)Math.Floor(locationVolume / materialPacketVolume) : 0;
        }

        #endregion

        #region Dispose Method

        /// <summary>
        /// Clean up after using this class .
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

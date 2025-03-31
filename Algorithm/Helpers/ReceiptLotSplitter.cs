using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses;
using SLAPScheduling.Domain.Enum;
using Material = SLAPScheduling.Domain.AggregateModels.MaterialAggregate.Materials.Material;

namespace SLAPScheduling.Algorithm.Helpers
{
    public class ReceiptLotSplitter : IDisposable
    {
        private List<InventoryReceiptEntry> entries { get; set; }
        private Dictionary<string, Material> materialDictionary { get; set; }
        private double locationVolume { get; set; }

        #region Constructor

        public ReceiptLotSplitter(List<InventoryReceiptEntry> entries, List<Material> materials, Warehouse warehouse)
        {
            this.entries = entries;
            materialDictionary = materials?.Count > 0 ? materials.ToDictionary(x => x.materialId, y => y) : new Dictionary<string, Material>();
            locationVolume = warehouse?.locations?.Count > 0 ? warehouse.GetLocationVolume() : 0;
        }

        #endregion

        #region Split Receipt Entries to Sublots

        /// <summary>
        /// Split receipt entries to multiple sublots (with empty location) based on the volume size.
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<ReceiptSublot> GetReceiptSubLots()
        //{
        //    foreach (var entry in entries)
        //    {
        //        if (materialDictionary.TryGetValue(entry.materialId, out Material? material))
        //        {
        //            ReceiptLot receiptLot = new ReceiptLot(receiptLotId: entry.purchaseOrderNumber,
        //                                                   material: material,
        //                                                   importedQuantity: entry.receiptLot.ImportedQuantity,
        //                                                   receiptLotStatus: LotStatus.Approved);

        //            entry.AddReceiptLot(receiptLot);

        //            int quantityPerLocation = CalculateQuantityPerLocation(receiptLot.Material);
        //            int subLotCount = CalculateNumberOfSubLot(receiptLot.ImportedQuantity, quantityPerLocation);

        //            for (int subLotIndex = 0; subLotIndex < subLotCount; subLotIndex++)
        //            {
        //                var subLotId = $"{entry.purchaseOrderNumber}_{subLotIndex}";
        //                var subLotQuantity = subLotIndex == subLotCount - 1 ? receiptLot.ImportedQuantity % quantityPerLocation : quantityPerLocation;

        //                yield return new ReceiptSublot(receiptSublotId: subLotId,
        //                                               material: receiptLot.Material,
        //                                               location: null,
        //                                               importedQuantity: subLotQuantity);
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Calculate the locations which 
        /// </summary>
        /// <param name="lotQuantity"></param>
        /// <param name="quantityPerLocation"></param>
        /// <returns></returns>
        private int CalculateNumberOfSubLot(double lotQuantity, int quantityPerLocation)
        {
            return quantityPerLocation > 0 ? (int)Math.Ceiling(lotQuantity / quantityPerLocation) : 0;
        }

        /// <summary>
        /// Calculate the quantity of material per location.
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        private int CalculateQuantityPerLocation(Material material)
        {
            var materialVolume = material.GetVolume();
            return locationVolume > 0 && materialVolume > 0 ? (int)Math.Floor(locationVolume / materialVolume) : 0;
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

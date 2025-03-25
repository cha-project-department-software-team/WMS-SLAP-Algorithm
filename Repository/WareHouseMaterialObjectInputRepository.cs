using SLAP.AggregateModels.WareHouseMaterialAggregate;

namespace SLAP.Repository
{
    public class WareHouseMaterialObjectInputRepository : IWareHouseMaterialObjectInputRepository
    {
        public static List<WareHouseMaterialObjectInput> listWareHouseMaterialObjects = new List<WareHouseMaterialObjectInput>();
        public WareHouseMaterialObjectInput Add(WareHouseMaterialObjectInput wareHouseMaterial)
        {
            listWareHouseMaterialObjects.Add(wareHouseMaterial);
            return wareHouseMaterial;
        }
    }
}

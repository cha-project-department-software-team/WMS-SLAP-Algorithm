using SLAP.AggregateModels.WareHouseMaterialAggregate;

namespace SLAP.AggregateModels.WorkAggregate
{
    public class MaterialOnWork
    {
        public MaterialInforOnWork? materialInfo { get; set; }
        public string? quantity { get; set; }
    }

    public class MaterialInforOnWork
    {
        public string? code { get; set; }
        public string? name { get; set; }
    }
}

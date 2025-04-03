namespace SLAPScheduling.Domain.AggregateModels.StorageAggregate.Warehouses
{
    public class WarehouseProperty : IAggregateRoot
    {
        [Key]
        public string propertyId { get; set; }

        public string propertyName { get; set; }
        public string propertyValue { get; set; }
        public UnitOfMeasure unitOfMeasure { get; set; }

        [ForeignKey("warehouseId")]
        public string warehouseId { get; set; }
        public Warehouse warehouse { get; set; }

        public WarehouseProperty(string propertyId, string propertyName, string propertyValue, UnitOfMeasure unitOfMeasure, string warehouseId)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyValue = propertyValue;
            this.unitOfMeasure = unitOfMeasure;
            this.warehouseId = warehouseId;
        }
    }
}

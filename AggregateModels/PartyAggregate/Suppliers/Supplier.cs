using SLAP.AggregateModels.InventoryReceiptAggregate;

namespace SLAP.AggregateModels.PartyAggregate
{
    public class Supplier  
    {
        public string supplierId { get; set; }
        public string supplierName { get; set; }
        public string address { get; set; }
        public string contactDetails { get; set; }

        public Supplier(string supplierId, string supplierName, string address, string contactDetails)
        {
            this.supplierId = supplierId;
            this.supplierName = supplierName;
            this.address = address;
            this.contactDetails = contactDetails;
        }
    }
}

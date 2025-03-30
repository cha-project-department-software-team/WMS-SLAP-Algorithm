using SLAPScheduling.Domain.AggregateModels.InventoryReceiptAggregate;
using SLAPScheduling.Domain.Seedwork;
using System.ComponentModel.DataAnnotations;

namespace SLAPScheduling.Domain.AggregateModels.PartyAggregate.Suppliers
{
    public class Supplier : Entity, IAggregateRoot
    {
        [Key]
        public string supplierId { get; set; }
        public string supplierName { get; set; }
        public string address { get; set; }
        public string contactDetails { get; set; }
        public List<InventoryReceipt> inventoryReceipts { get; set; }

        public Supplier(string supplierId, string supplierName, string address, string contactDetails)
        {
            this.supplierId = supplierId;
            this.supplierName = supplierName;
            this.address = address;
            this.contactDetails = contactDetails;
        }

        public void UpdateSupplier(string supplierName, string address, string contactDetails)
        {
            this.supplierName = supplierName;
            this.address = address;
            this.contactDetails = contactDetails;
        }


    }
}

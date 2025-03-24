namespace TabuSearchProductionScheduling.Classes
{
    public class DataSource
    {
        public List<BOM> BOMs { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Product> Products { get; set; }
        public List<WorkCenter> WorkCenters { get; set; }
        public List<Order> Orders { get; set; }

        public DataSource(List<BOM> bOMs, List<Customer> customers, List<Product> products, List<WorkCenter> workCenters, List<Order> orders)
        {
            BOMs = bOMs;
            Customers = customers;
            Products = products;
            WorkCenters = workCenters;
            Orders = orders;
        }

        public List<WorkOrder> GetWorkOrders()
        {
            var workOrders = new List<WorkOrder>();
            foreach(var order in this.Orders)
            {
                var product = order.Product;
                var operation = order.Product.BOM.Operation;
                WorkOrder workOrder = new WorkOrder(id: this.Orders.IndexOf(order),
                                                    priority: order.Customer.Weight,
                                                    mold: product.Mold,
                                                    productCode: product.Reference,
                                                    processingTime: order.Quantity * operation.ManualDuration,
                                                    workCenter: operation.WorkCenter,
                                                    alternativeWorkCenters: operation.AlternativeWorkCenters,
                                                    bom: product.BOM,
                                                    releaseDate: order.ReleaseDate,
                                                    dueDate: order.DueDate,
                                                    startDate: DateTime.Now);

                workOrders.Add(workOrder);
            }

            return workOrders;
        }
    }
}

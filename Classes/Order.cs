namespace TabuSearchProductionScheduling.Classes
{
    public class Order
    {
        public DateTime DueDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public double Quantity { get; set; }

        public Order(DateTime dueDate, DateTime releaseDate, Customer customer, Product product, double quantity)
        {
            DueDate = dueDate;
            ReleaseDate = releaseDate;
            Customer = customer;
            Product = product;
            Quantity = quantity;
        }
    }
}

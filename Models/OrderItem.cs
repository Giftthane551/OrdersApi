namespace OrdersApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }                   // Primary key
        public Guid OrderId { get; set; }             // Foreign key to Order
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }        // Will use decimal(18,2) in DbContext
    }
}

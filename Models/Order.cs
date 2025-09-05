namespace OrdersApi.Models
{
    public enum OrderStatus { Draft, Submitted, Completed }

    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int OrderNumber { get; set; }         // Auto-increment via DbContext
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Draft;

        public List<OrderItem> Items { get; set; } = new();
    }
}

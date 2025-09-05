using System.ComponentModel.DataAnnotations;

namespace OrdersApi.Dtos
{
    public class CreateOrderDto
    {
        [Required]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        [Required] public string ProductName { get; set; } = default!;
        [Range(1, int.MaxValue)] public int Quantity { get; set; }
        [Range(0.0, double.MaxValue)] public decimal UnitPrice { get; set; }
    }
}

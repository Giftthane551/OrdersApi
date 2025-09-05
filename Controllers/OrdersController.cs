using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersApi.Data;
using OrdersApi.Dtos;
using OrdersApi.Models;

namespace OrdersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersDbContext _db;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrdersDbContext db, ILogger<OrdersController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // POST: api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
            {
                _logger.LogWarning("Attempted to create order with no items");
                return BadRequest(new { error = "An order must have at least one item." });
            }

            int nextOrderNumber;

            // Check if the database provider supports sequences (PostgreSQL)
            if (_db.Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                nextOrderNumber = 0; // Let PostgreSQL sequence handle it
            }
            else
            {
                // InMemory or other providers: calculate next order number manually
                nextOrderNumber = await _db.Orders.MaxAsync(o => (int?)o.OrderNumber) ?? 0;
                nextOrderNumber++;
            }

            var order = new Order
            {
                OrderNumber = nextOrderNumber > 0 ? nextOrderNumber : 0, // 0 if PostgreSQL will handle
                Items = dto.Items.Select(i => new OrderItem
                {
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Order created: Id={OrderId} Number={OrderNumber}", order.Id, order.OrderNumber);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }


        // GET: api/orders/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                _logger.LogInformation("Order {OrderId} not found", id);
                return NotFound();
            }
            return Ok(order);
        }

        // GET: api/orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var list = await _db.Orders.Include(o => o.Items).OrderBy(o => o.OrderNumber).ToListAsync();
            return Ok(list);
        }

        // PUT: api/orders/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] CreateOrderDto dto)
        {
            var existing = await _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            if (existing == null) return NotFound();

            if (dto.Items == null || dto.Items.Count == 0)
            {
                _logger.LogWarning("Attempted to update order {OrderId} with no items", id);
                return BadRequest(new { error = "An order must have at least one item." });
            }

            _db.OrderItems.RemoveRange(existing.Items);
            existing.Items = dto.Items.Select(i => new OrderItem
            {
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            await _db.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} updated", id);
            return Ok(existing);
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var existing = await _db.Orders.FindAsync(id);
            if (existing == null) return NotFound();

            _db.Orders.Remove(existing);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Order {OrderId} deleted", id);
            return NoContent();
        }
    }
}

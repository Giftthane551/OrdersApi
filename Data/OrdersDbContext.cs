using Microsoft.EntityFrameworkCore;
using OrdersApi.Models;

namespace OrdersApi.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PostgreSQL sequence for order numbers
            modelBuilder.HasSequence<int>("OrderNumbers").StartsAt(1).IncrementsBy(1);

            modelBuilder.Entity<Order>(b =>
            {
                b.HasKey(o => o.Id);
                b.Property(o => o.OrderNumber)
                 .HasDefaultValueSql("nextval('\"OrderNumbers\"')");
                b.Property(o => o.Status)
                 .HasConversion<string>();
                b.HasMany(o => o.Items)
                 .WithOne()
                 .HasForeignKey(i => i.OrderId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.HasKey(i => i.Id);
                b.Property(i => i.Id)
                 .UseIdentityColumn(); // PostgreSQL identity
                b.Property(i => i.UnitPrice)
                 .HasColumnType("numeric(18,2)"); // PostgreSQL decimal
            });
        }
    }
}

using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.CourierAggregate;
using DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Adapters.Postgres;

public class ApplicationDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Courier> Couriers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Order Aggregate
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderStatusEntityTypeConfiguration());

        // Order statuses
        modelBuilder.Entity<OrderStatus>(b =>
        {
            var allStatuses = OrderStatus.List();
            b.HasData(allStatuses.Select(c => new {c.Id, c.Name}));
        });


        modelBuilder.ApplyConfiguration(new CourierEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CourierStatusEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransportEntityTypeConfiguration());

        // Courier statuses
        modelBuilder.Entity<CourierStatus>(b =>
        {
            var allStatuses = CourierStatus.List();
            b.HasData(allStatuses.Select(c => new {c.Id, c.Name}));
        });

        // Courier transports
        modelBuilder.Entity<Transport>(b =>
        {
            var allTransports = Transport.List();
            b.HasData(allTransports.Select(c => new {c.Id, c.Name, c.Speed, c.Capacity.Value}));
        });
    }
}
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

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseNpgsql("postgresql://postgres:secret@localhost:5433");
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Order Aggregate
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());

        modelBuilder.ApplyConfiguration(new CourierEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransportEntityTypeConfiguration());
        

        // Courier transports
        modelBuilder.Entity<Transport>(b =>
        {
            var allTransports = Transport.All;
            b.HasData(allTransports.Select(c => new {c.Id, c.Name, c.Speed, c.Capacity.Kilograms}));
        });
    }
}
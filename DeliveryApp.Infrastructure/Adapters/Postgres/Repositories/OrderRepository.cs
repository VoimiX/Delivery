using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<Order> AddOrder(Order order)
    {
        var entity = _dbContext.Orders.Add(order).Entity;
        return Task.FromResult(entity);
    }

    public async Task<Order> GetOrder(Guid id)
    {
        var order = await _dbContext
            .Orders
            .FirstOrDefaultAsync(o => o.Id == id);

        return order;

    }

    public async Task<Order[]> GetOrdersAssigned()
    {
        var orders = await _dbContext
            .Orders
            .Where(o => o.Status == OrderStatus.Assigned)
            .ToArrayAsync();

        return orders;
    }

    public async Task<Order[]> GetOrdersNew()
    {
        var orders = await _dbContext
            .Orders
            .Where(o => o.Status == OrderStatus.Created)
            .ToArrayAsync();

        return orders;
    }

    public Task UpdateOrder(Order order)
    {
        _dbContext.Entry(order).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}

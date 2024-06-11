using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;

public class CourierRepository : ICourierRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CourierRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public Task<Courier> AddCourier(Courier courier)
    {
        if (courier.Transport != null) _dbContext.Attach(courier.Transport);
        var entity = _dbContext.Couriers.Add(courier).Entity;
        return Task.FromResult(entity);
    }

    public async Task<Courier> GetCourier(Guid id)
    {
        var courier = await _dbContext
           .Couriers
           .FirstOrDefaultAsync(o => o.Id == id);

        return courier;
    }

    public async Task<Courier[]> GetFreeCouriers()
    {
        var couriers = await _dbContext
           .Couriers
           .Include(c => c.Transport)
           .Where(o => o.Status == CourierStatus.Ready)
           .ToArrayAsync();

        return couriers;
    }

    public async Task<Courier[]> GetAssignedCouriers()
    {
        var couriers = await _dbContext
           .Couriers
           .Include(c => c.Transport)
           .Where(c => c.Status == CourierStatus.Busy)
           .Join(_dbContext.Orders,
            c => c.Id,
            o => o.CourierId,
            (c, o) => new
            {
                Courier = c,
                Order = o
            })           
           .Where(x => x.Order.Status == OrderStatus.Assigned)
           .ToArrayAsync();

        foreach (var courier in couriers)
        {
            courier.Courier.SetOrder(courier.Order);
        }

        return couriers.Select(c => c.Courier).ToArray();
    }

    public Task UpdateCourier(Courier courier)
    {
        _dbContext.Entry(courier).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}

using DeliveryApp.Core.Domain.CourierAggregate;
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
           .Where(o => o.Status == CourierStatus.Ready)
           .ToArrayAsync();

        return couriers;
    }

    public Task UpdateCourier(Courier courier)
    {
        _dbContext.Entry(courier).State = EntityState.Modified;
        return Task.CompletedTask;
    }
}

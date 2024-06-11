using MediatR;
using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public UnitOfWork(ApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
        await PublishDomainEventsAsync();
        return true;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEntities = _dbContext.ChangeTracker
            .Entries<Aggregate>()
            .Where(x => x.Entity.GetDomainEvents().Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.GetDomainEvents())
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }
}
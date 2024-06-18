using DeliveryApp.Infrastructure.Adapters.Postgres.Entities;
using MediatR;
using Newtonsoft.Json;
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
        await SaveDomainEventsInOutboxEventsAsync();

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

    private async Task SaveDomainEventsInOutboxEventsAsync()
    {
        // Достаем доменные события из Aggregate и преобразовываем их к OutboxMessage
        var outboxMessages = _dbContext.ChangeTracker
            .Entries<Aggregate>()
            .Select(x => x.Entity)
            .SelectMany(aggregate =>
            {
                var domainEvents = aggregate.GetDomainEvents();
                aggregate.ClearDomainEvents();
                return domainEvents;
            }
            )
            .Select(domainEvent => new OutboxMessage
            {
                Id = domainEvent.EventId,
                OccuredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        // Добавяляем OutboxMessage в dbContext, после выхода из метода они и сам Aggregate будут сохранены
        await _dbContext.Set<OutboxMessage>().AddRangeAsync(outboxMessages);
    }

}
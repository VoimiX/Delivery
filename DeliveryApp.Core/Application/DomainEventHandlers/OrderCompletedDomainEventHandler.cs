using DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;
using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.DomainEventHandlers;

public class OrderCompletedDomainEventHandler : INotificationHandler<OrderCompletedDomainEvent>
{
    private readonly IBusProducer _busProducer;

    public OrderCompletedDomainEventHandler(IBusProducer busProducer)
    {
        _busProducer = busProducer;
    }

    public async Task Handle(OrderCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _busProducer.PublishOrderCompleted(notification);
    }
}

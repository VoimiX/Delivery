using DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;

namespace DeliveryApp.Core.Ports;

public interface IBusProducer
{
    Task PublishOrderAssigned(OrderAssignedDomainEvent notification);
    Task PublishOrderCompleted(OrderCompletedDomainEvent notification);
    Task PublishOrderCreated(OrderCreatedDomainEvent notification);
}

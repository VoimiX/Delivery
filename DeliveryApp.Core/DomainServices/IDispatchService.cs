using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;

namespace DeliveryApp.Core.DomainServices;

public interface IDispatchService
{
    Task<Courier> Dispatch(Order order, IReadOnlyList<Courier> couriers);
}

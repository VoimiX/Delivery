using DeliveryApp.Core.Domain.OrderAggregate;

namespace DeliveryApp.Core.Ports;

public interface IOrderRepository
{
    Task<Order> AddOrder(Order order);
    Task UpdateOrder(Order order);
    Task<Order> GetOrder(Guid id);
    Task<Order[]> GetOrdersNew();
    Task<Order[]> GetOrdersAssigned();
}

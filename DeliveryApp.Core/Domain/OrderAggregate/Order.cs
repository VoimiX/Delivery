using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;
using DeliveryApp.Core.Domain.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate
{
    public class Order : Aggregate
    {
        private Order() { }

        public Order(Guid id, Location location, Weight weight)
        {
            if (id == Guid.Empty) throw new ArgumentException("Empty paramater", nameof(id));
            if (location == null) throw new ArgumentNullException(nameof(location));
            if (weight == null) throw new ArgumentNullException(nameof(weight));

            Id = id;
            Location = location;
            Weight = weight;

            Status = OrderStatus.Created;

            RaiseDomainEvent(new OrderCreatedDomainEvent(OrderId: id, Weight: weight.Kilograms));
        }

        public Guid? CourierId { get; private set; }
        public Location Location { get; }
        public Weight Weight { get; }
        public OrderStatus Status { get; private set; }

        public void AssignToCourier(Courier courier)
        {
            if (courier == null) throw new ArgumentNullException(nameof (courier));

            Status = OrderStatus.Assigned;
            courier.SetStatus(CourierStatus.Busy);
            courier.SetOrder(this);
            CourierId = courier.Id;

            RaiseDomainEvent(new OrderAssignedDomainEvent(OrderId: Id, CourierId: CourierId.Value));
        }

        public void Complete(Courier courier)
        {
            if (courier == null) throw new ArgumentNullException(nameof(courier));
            if (courier.Id != CourierId) throw new DeliveryException("Неверный курьер для завершения заказа");

            if (Status != OrderStatus.Assigned)
            {
                throw new DeliveryException("Завершить можно только назначенный ранее заказ");
            }            

            Status = OrderStatus.Completed;
            courier.SetStatus(CourierStatus.Ready);

            RaiseDomainEvent(new OrderCompletedDomainEvent(OrderId: Id));
        }
    }
}

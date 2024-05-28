using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.Exceptions;
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
        }

        public Courier Courier { get; private set; }
        public Location Location { get; }
        public Weight Weight { get; }
        public OrderStatus Status { get; private set; }

        public void AssignToCourier(Courier courier)
        {
            Courier = courier;
            Status = OrderStatus.Assigned;
            courier.SetStatus(CourierStatus.Busy);
        }

        public void Complete()
        {
            if (Status != OrderStatus.Assigned)
            {
                throw new DeliveryException("Завершить можно только назначенный ранее заказ");
            }

            Status = OrderStatus.Completed;            
            Courier.SetStatus(CourierStatus.Ready);
        }
    }
}

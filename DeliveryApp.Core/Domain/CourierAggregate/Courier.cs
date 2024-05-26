using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Domain.OrderAggregate;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Courier
    {
        public Courier(int id, string name, Transport transport)
        {
            Id = id;
            Name = name;
            Transport = transport;
            Location = new Location(1, 1);
            Status = CourierStatus.NotAvailable;
        }

        private Courier() { }

        public int Id { get; }
        public string Name { get; }
        public Transport Transport { get; }
        public Location Location { get; }
        public CourierStatus Status { get; private set; }
        public Order Order { get; private set; }
        public float? StepsDistanceToOrder
        {
            get
            {
                if (Order == null) return null;

                var steps = (float)Location.DistanceTo(Order.Location) / Transport.Speed;
                return steps;
            }
        }

        public void SetStatus(CourierStatus status)
        {
            Status = status;
        }

        public void StartWork(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (order.Courier != this) throw new DeliveryException("Неправильный заказ, назначен другому курьеру.");

            Order = order;

            if (Status == CourierStatus.Busy)
            {
                throw new DeliveryException("Курьер занят, невозможно начать работу.");
            }

            Status = CourierStatus.Ready;
        }

        public void EndWork()
        {
            if (Status == CourierStatus.Busy)
            {
                throw new DeliveryException("Курьер занят, невозможно завершить работу.");
            }
            if (Status == CourierStatus.NotAvailable)
            {
                throw new DeliveryException("Курьер недоступен, невозможно завершить работу.");
            }

            Status = CourierStatus.NotAvailable;
        }

        public void MakeStepToOrder()
        {

        }
    }
}

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
        public Location Location { get; private set; }
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

        public void AssignOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            if (!Transport.CanHandleWeight(order.Weight))
            {
                throw new DeliveryException($"Транспорт курьера {Transport} не позволяет по весу перевозить такой заказ.");
            }

            order.AssignToCourier(this);

            Order = order;
        }

        public void StartWork()
        {
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
            if (Status == CourierStatus.NotAvailable)
            {
                throw new DeliveryException($"Невозможно сделать шаг к заказу, неверный статус курьера ({Status}).");
            }

            Status = CourierStatus.Busy;

            int newX = Location.X, newY = Location.Y;

            var restSpeed = Transport.Speed;

            var diffX = Location.X - Order.Location.X;
            var x = diffX > 0 ? 1 : -1;
            
            int distX = Math.Min(Math.Abs(diffX), restSpeed);
            
            restSpeed = restSpeed - distX;

            newX = Location.X + distX * -1 * x;

            if (restSpeed > 0)
            {
                var diffY = Location.Y - Order.Location.Y;
                var y = diffY > 0 ? 1 : -1;

                int distY = Math.Min(Math.Abs(diffY), restSpeed);
                newY = Location.Y + distY * -1 * y;
            }

            Location = new Location(newX, newY);

            if (Location == Order.Location)
            {
                Status = CourierStatus.Ready;
                Order.Complete();
            }
        }
    }
}

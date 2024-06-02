using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Domain.OrderAggregate;
using Primitives;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Courier : Aggregate
    {
        public Courier(Guid id, string name, Transport transport)
        {
            if (id == Guid.Empty) throw new ArgumentException("Empty parameter", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (transport == null) throw new ArgumentNullException(nameof(transport));

            Id = id;
            Name = name;
            Transport = transport;
            Location = new Location(1, 1);
            Status = CourierStatus.NotAvailable;
        }

        private Courier() { }

        public string Name { get; }
        public Transport Transport { get; }
        public Location Location { get; private set; }
        public CourierStatus Status { get; private set; }
        public Guid? OrderId { get; private set; }

        public float GetStepsDistanceToOrder(Location orderLocation)
        {
            if (orderLocation == null) throw new ArgumentNullException(nameof(orderLocation));

            var steps = (float)Location.DistanceTo(orderLocation) / Transport.Speed;
            return steps;
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
            OrderId = order.Id;
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

        public void MakeStepToOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (order.Id != OrderId) throw new DeliveryException("Передан неверный заказ для шага к заказу");

            if (Status == CourierStatus.NotAvailable)
            {
                throw new DeliveryException($"Невозможно сделать шаг к заказу, неверный статус курьера ({Status}).");
            }
            if (order.Status != OrderStatus.Assigned)
            {
                throw new DeliveryException($"Невозможно сделать шаг к заказу, неверный статус заказа ({order.Status}).");
            }

            if (order.Location == Location)
            {
                Status = CourierStatus.Ready;
                order.Complete(this);
            }

            Status = CourierStatus.Busy;

            int newX = Location.X, newY = Location.Y;

            var restSpeed = Transport.Speed;

            var diffX = Location.X - order.Location.X;
            var x = diffX > 0 ? -1 : 1;

            int distX = Math.Min(Math.Abs(diffX), restSpeed);

            restSpeed = restSpeed - distX;

            newX = Location.X + distX * x;

            if (restSpeed > 0)
            {
                var diffY = Location.Y - order.Location.Y;
                var y = diffY > 0 ? -1 : 1;

                int distY = Math.Min(Math.Abs(diffY), restSpeed);
                newY = Location.Y + distY * y;
            }

            Location = new Location(newX, newY);

            if (Location == order.Location)
            {
                Status = CourierStatus.Ready;
                order.Complete(this);
            }
        }
    }
}

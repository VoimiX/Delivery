using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Car : Transport
    {
        public Car(int id, string name)
            : base(id, name, speed: 4, capacity: new Weight(20))
        {
        }
    }
}

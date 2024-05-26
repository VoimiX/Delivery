using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Pedestrian : Transport
    {
        public Pedestrian(int id, string name)
            : base(id, name, speed: 1, capacity: new Weight(1))
        {
        }
    }
}

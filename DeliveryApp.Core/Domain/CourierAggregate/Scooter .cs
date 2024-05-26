using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Scooter : Transport
    {
        public Scooter(int id, string name)
            : base(id, name, speed: 3, capacity: new Weight(6))
        {
        }
        public override bool CanHandleWeight(Weight weight)
        {
            throw new NotImplementedException();
        }
    }
}

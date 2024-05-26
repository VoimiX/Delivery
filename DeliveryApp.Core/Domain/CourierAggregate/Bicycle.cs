using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Bicycle : Transport
    {
        public Bicycle(int id, string name)
            : base(id, name, speed: 2, capacity: new Weight(4))
        {
        }

        public override bool CanHandleWeight(Weight weight)
        {
            throw new NotImplementedException();
        }
    }
}

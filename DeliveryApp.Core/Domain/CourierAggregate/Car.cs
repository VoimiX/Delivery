using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Car : Transport
    {
        public Car(int id, string name)
            : base(id, name, speed: 4, capacity: new Weight(8))
        {
        }

        public override bool CanHandleWeight(Weight weight)
        {
            throw new NotImplementedException();
        }
    }
}

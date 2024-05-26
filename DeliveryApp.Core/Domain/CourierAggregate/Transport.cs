using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public abstract class Transport
    {
        private Transport() { }
        protected Transport(int id, string name, int speed, Weight capacity)
        {
            Id = id;
            Name = name;
            Speed = speed;
            Capacity = capacity;
        }

        public int Id { get; }
        public string Name { get; }
        public int Speed { get; }
        public Weight Capacity { get; }

        public abstract bool CanHandleWeight(Weight weight);
    }
}

using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Transport
    {
        private Transport() { }
        protected Transport(int id, int speed, Weight capacity, TransportType type)
        {
            Id = id;
            Speed = speed;
            Capacity = capacity;
            Type = type;
        }

        public int Id { get; }
        public int Speed { get; }
        public Weight Capacity { get; }
        public TransportType Type { get; }

        public override string ToString()
        {
            return Type.ToString();
        }

        public bool CanHandleWeight(Weight weight)
        {
            return weight <= Capacity;
        }

        public static Transport[] All
        {
            get
            {
                return
                [
                    new Transport(1, speed: 4, capacity: new Weight(8), TransportType.Car),
                    new Transport(2, speed: 1, capacity: new Weight(1), TransportType.Pedestrian),
                    new Transport(3, speed: 2, capacity: new Weight(4), TransportType.Bicycle),
                    new Transport(4, speed: 3, capacity: new Weight(6), TransportType.Scooter),
                ];
            }
        }

        public enum TransportType
        {
            Pedestrian = 1,
            Car = 2,
            Scooter = 3,
            Bicycle = 4
        }

        public static Transport Car => All.Single(x => x.Type == TransportType.Car);

        public static Transport Pedestrain => All.Single(x => x.Type == TransportType.Pedestrian);          
        
        public static Transport Scooter => All.Single(x => x.Type == TransportType.Scooter);

        public static Transport Bicycle => All.Single(x => x.Type == TransportType.Bicycle);
    }
}

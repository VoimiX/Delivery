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

        public bool CanHandleWeight(Weight weight)
        {
            return weight <= Capacity;
        }

        public static Transport[] All
        {
            get
            {
                var assembly = typeof(Transport).Assembly;
                var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Transport))).ToList();

                var list = new List<Transport>();
                foreach (var type in types)
                {
                    int id = types.IndexOf(type) + 1;
                    string name = type.Name;
                    Transport instance = (Transport)Activator.CreateInstance(
                        type, new object[] { id, name });
                    list.Add(instance);
                }

                return list.ToArray();
            }
        }
    }
}

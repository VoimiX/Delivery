namespace DeliveryApp.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ItemValueAttribute : Attribute
    {
        public ItemValueAttribute(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}

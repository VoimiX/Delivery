using DeliveryApp.Core.Attributes;

namespace DeliveryApp.Core.Domain.OrderAggregate
{
    public enum OrderStatus
    {
        [ItemValue(1)]
        Created,

        [ItemValue(2)]
        Assigned,

        [ItemValue(3)]
        Completed
    }
}

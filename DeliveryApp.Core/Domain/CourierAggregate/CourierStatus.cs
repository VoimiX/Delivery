using DeliveryApp.Core.Attributes;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public enum CourierStatus
    {
        [ItemValue(1)]
        NotAvailable,

        [ItemValue(2)]
        Ready,

        [ItemValue(3)]
        Busy
    }
}

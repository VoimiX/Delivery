using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveToOrder
{
    public class MoveToOrderCommand : IRequest<MoveToOrderResponse>
    {
        public MoveToOrderCommand(IReadOnlyList<Guid> couriers)
        {
            if (couriers == null) throw new ArgumentNullException(nameof(couriers));
            if (couriers.Count == 0) throw new ArgumentException("Пустой список курьеров", nameof(couriers));

            Couriers = couriers;
        }

        public IReadOnlyList<Guid> Couriers { get; }
    }
}

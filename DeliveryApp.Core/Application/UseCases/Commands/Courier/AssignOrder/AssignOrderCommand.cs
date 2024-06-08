using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.AssignOrder
{
    public class AssignOrderCommand : IRequest<AssignOrderResponse>
    {
        public AssignOrderCommand(IReadOnlyList<Guid> couriers, Guid orderId)
        {
            Couriers = couriers;
            OrderId = orderId;
        }

        public IReadOnlyList<Guid> Couriers { get; }
        public Guid OrderId { get; }
    }
}

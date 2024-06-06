using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Couriers.AssignOrder
{
    public class AssignOrderCommand : IRequest<AssignOrderResponse>
    {
        public AssignOrderCommand(Guid courierId, Guid orderId)
        {
            CourierId = courierId;
            OrderId = orderId;
        }

        public Guid CourierId { get; }
        public Guid OrderId { get; }
    }
}

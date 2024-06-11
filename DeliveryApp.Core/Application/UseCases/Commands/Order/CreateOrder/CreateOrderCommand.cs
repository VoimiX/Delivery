using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public CreateOrderCommand(Guid orderId, string address, int weight)
        {
            OrderId = orderId;
            Address = address;
            Weight = weight;
        }

        public Guid OrderId { get; }
        public string Address { get; }
        public int Weight { get; }
    }
}

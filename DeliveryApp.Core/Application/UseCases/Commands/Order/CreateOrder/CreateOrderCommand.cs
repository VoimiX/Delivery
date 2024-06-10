using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public CreateOrderCommand(Guid id, string address, int weight)
        {
            Id = id;
            Address = address;
            Weight = weight;
        }

        public Guid Id { get; }
        public string Address { get; }
        public int Weight { get; }
    }
}

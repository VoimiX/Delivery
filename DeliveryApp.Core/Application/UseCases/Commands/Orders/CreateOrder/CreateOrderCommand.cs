using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public CreateOrderCommand(Guid id, int locationX, int locationY, int weight)
        {
            Id = id;
            LocationX = locationX;
            LocationY = locationY;
            Weight = weight;
        }

        public Guid Id { get; }
        public int LocationX { get; }
        public int LocationY { get; }
        public int Weight { get; }
    }
}

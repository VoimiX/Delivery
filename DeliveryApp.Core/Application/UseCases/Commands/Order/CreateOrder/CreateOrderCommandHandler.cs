using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Ports;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var createdOrder =  await _orderRepository.AddOrder(new Domain.OrderAggregate.Order
            (
                request.Id, new Location(request.LocationX, request.LocationY), new Weight(request.Weight))
            );

        return new CreateOrderResponse(createdOrder.Id);
    }
}

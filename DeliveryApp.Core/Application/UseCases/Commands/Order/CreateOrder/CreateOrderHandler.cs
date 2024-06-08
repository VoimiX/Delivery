using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var createdOrder =  await _orderRepository.AddOrder(new Domain.OrderAggregate.Order
            (
                request.Id, new Location(request.LocationX, request.LocationY), new Weight(request.Weight))
            );

        await _unitOfWork.SaveEntitiesAsync();

        return new CreateOrderResponse(createdOrder.Id);
    }
}

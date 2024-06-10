using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.Order.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeoServiceClient _geoServiceClient;

    public CreateOrderHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IGeoServiceClient geoServiceClient)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _geoServiceClient = geoServiceClient;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var geoLocation = await _geoServiceClient.GetAddressLocation(request.Address);

        var createdOrder =  await _orderRepository.AddOrder(new Domain.OrderAggregate.Order
            (
                request.Id, geoLocation, new Weight(request.Weight))
            );

        await _unitOfWork.SaveEntitiesAsync();

        return new CreateOrderResponse(createdOrder.Id);
    }
}

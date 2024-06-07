using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.AssignOrder;

public class AssignOrderHandler : IRequestHandler<AssignOrderCommand, AssignOrderResponse>
{
    private readonly ICourierRepository _courierRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignOrderHandler(IUnitOfWork unitOfWork, ICourierRepository courierRepository, IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _courierRepository = courierRepository;
        _orderRepository = orderRepository;
    }

    public async Task<AssignOrderResponse> Handle(AssignOrderCommand request, CancellationToken cancellationToken)
    {
        var courier = await _courierRepository.GetCourier(request.CourierId);
        if (courier == null) throw new DeliveryException($"Курьер не найден по id={request.CourierId}");

        var order = await _orderRepository.GetOrder(request.OrderId);
        if (order == null) throw new DeliveryException($"Заказ не найден по id={request.OrderId}");

        order.AssignToCourier(courier);
        await _orderRepository.UpdateOrder(order);

        await _unitOfWork.SaveEntitiesAsync();

        return new AssignOrderResponse();
    }
}

using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveToOrder;

public class MoveToOrderCommandHandler : IRequestHandler<MoveToOrderCommand, MoveToOrderResponse>
{
    private readonly ICourierRepository _courierRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MoveToOrderCommandHandler(IUnitOfWork unitOfWork, ICourierRepository courierRepository, IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _courierRepository = courierRepository;
        _orderRepository = orderRepository;
    }

    public async Task<MoveToOrderResponse> Handle(MoveToOrderCommand request, CancellationToken cancellationToken)
    {
        var courier = await _courierRepository.GetCourier(request.CourierId);
        if (courier == null) throw new DeliveryException($"Курьер не найден по id={request.CourierId}");

        if (courier.OrderId == null) throw new DeliveryException($"У курьера id={request.CourierId} не назначен заказ. Невозможно сделать шаг к заказу.");

        var order = await _orderRepository.GetOrder(courier.OrderId.Value);
        if (order == null) throw new DeliveryException($"Заказ не найден по id={courier.OrderId}");

        courier.MakeStepToOrder(order);

        await _courierRepository.UpdateCourier(courier);
        await _unitOfWork.SaveEntitiesAsync();

        return new MoveToOrderResponse(courier.Location);
    }
}

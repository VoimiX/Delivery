using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveToOrder;

public class MoveToOrderHandler : IRequestHandler<MoveToOrderCommand, MoveToOrderResponse>
{
    private readonly ICourierRepository _courierRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MoveToOrderHandler(IUnitOfWork unitOfWork, ICourierRepository courierRepository, IOrderRepository orderRepository)
    {
        _unitOfWork = unitOfWork;
        _courierRepository = courierRepository;
        _orderRepository = orderRepository;
    }

    public async Task<MoveToOrderResponse> Handle(MoveToOrderCommand request, CancellationToken cancellationToken)
    {
        foreach(var courierId in request.Couriers)
        {
            var courier = await _courierRepository.GetCourier(courierId);
            if (courier == null) throw new DeliveryException($"Курьер не найден по id={courierId}");
            if (courier.OrderId == null) throw new DeliveryException($"У курьера id={courierId} не назначен заказ. Невозможно сделать шаг к заказу.");

            var order = await _orderRepository.GetOrder(courier.OrderId.Value);
            if (order == null) throw new DeliveryException($"Заказ не найден по id={courier.OrderId}");

            courier.MakeStepToOrder(order);

            await _courierRepository.UpdateCourier(courier);            
        }
        await _unitOfWork.SaveEntitiesAsync();

        return new MoveToOrderResponse();
    }
}

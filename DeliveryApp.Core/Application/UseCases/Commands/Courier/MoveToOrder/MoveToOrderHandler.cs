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
        var couriesInWork = await _courierRepository.GetBusyCouriers();

        foreach(var courier in couriesInWork)
        {
            var order = await _orderRepository.GetCourierOrder(courier.Id);
            if (order == null)
                continue; // пока не назначен заказ

            courier.SetOrder(order);
            courier.MakeStepToOrder(order);
            if (order.Status == Domain.OrderAggregate.OrderStatus.Completed)
            {
                await _orderRepository.UpdateOrder(order);
            }

            await _courierRepository.UpdateCourier(courier);
        }
        await _unitOfWork.SaveEntitiesAsync();

        return new MoveToOrderResponse();
    }
}

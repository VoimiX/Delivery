using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.DomainServices;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.AssignOrder;

public class AssignOrderHandler : IRequestHandler<AssignOrderCommand, AssignOrderResponse>
{
    private readonly ICourierRepository _courierRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IDispatchService _dispatchService;
    private readonly IUnitOfWork _unitOfWork;

    public AssignOrderHandler(
        IUnitOfWork unitOfWork,
        ICourierRepository courierRepository,
        IOrderRepository orderRepository,
        IDispatchService dispatchService)
    {
        _unitOfWork = unitOfWork;
        _courierRepository = courierRepository;
        _orderRepository = orderRepository;
        _dispatchService = dispatchService;
    }

    public async Task<AssignOrderResponse> Handle(AssignOrderCommand request, CancellationToken cancellationToken)
    {
        var couriers = new List<Domain.CourierAggregate.Courier>();
        foreach(var courierId in request.Couriers)
        {
            var courier = await _courierRepository.GetCourier(courierId);
            if (courier == null) throw new DeliveryException($"Курьер не найден по id={courierId}");

            couriers.Add(courier);
        }       

        var order = await _orderRepository.GetOrder(request.OrderId);
        if (order == null) throw new DeliveryException($"Заказ не найден по id={request.OrderId}");

        var bestCourier = await _dispatchService.Dispatch(order, couriers);
       
        await _courierRepository.UpdateCourier(bestCourier);
        await _orderRepository.UpdateOrder(order);

        await _unitOfWork.SaveEntitiesAsync();

        return new AssignOrderResponse();
    }
}

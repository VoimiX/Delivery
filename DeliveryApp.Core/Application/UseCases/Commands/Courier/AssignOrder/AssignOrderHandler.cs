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
        var orders = await _orderRepository.GetOrdersNew();
        var couriers = (await _courierRepository.GetFreeCouriers()).ToList();

        foreach(var order in orders)
        {
            if (couriers.Count == 0) break;

            var bestCourier = await _dispatchService.Dispatch(order, couriers);
            if (bestCourier == null) continue;

            await _courierRepository.UpdateCourier(bestCourier);
            await _orderRepository.UpdateOrder(order);

            couriers.Remove(bestCourier); // удаляем из списка доступных курьеров
        }
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return new AssignOrderResponse();
    }
}

using DeliveryApp.Core.Application.UseCases.Commands.Courier.StartWork;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.EndWork;

public class EndWorkHandler : IRequestHandler<EndWorkCommand, EndWorkResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICourierRepository _courierRepository;

    public EndWorkHandler(
        IUnitOfWork unitOfWork,
        ICourierRepository courierRepository)
    {
        _unitOfWork = unitOfWork;
        _courierRepository = courierRepository;
    }

    public async Task<EndWorkResponse> Handle(EndWorkCommand request, CancellationToken cancellationToken)
    {
        var courier = await _courierRepository.GetCourier(request.CourierId);
        courier.EndWork();

        await _courierRepository.UpdateCourier(courier);
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return new EndWorkResponse();
    }
}

using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.StartWork;

public class StartWorkHandler : IRequestHandler<StartWorkCommand, StartWorkResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICourierRepository _courierRepository;

    public StartWorkHandler(
        IUnitOfWork unitOfWork,
        ICourierRepository courierRepository)
    {
        _unitOfWork = unitOfWork;
        _courierRepository = courierRepository;
    }

    public async Task<StartWorkResponse> Handle(StartWorkCommand request, CancellationToken cancellationToken)
    {
        var courier = await _courierRepository.GetCourier(request.CourierId);
        courier.StartWork();

        await _courierRepository.UpdateCourier(courier);
        await _unitOfWork.SaveEntitiesAsync(cancellationToken);

        return new StartWorkResponse();
    }
}

using DeliveryApp.Core.Application.UseCases.Queries.Couriers.Dto;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.Couriers.GetCouriesReadyBusy;

public class GetCouriesReadyBusyResponse : IRequest<GetCouriesReadyBusyResponse>
{
    public GetCouriesReadyBusyResponse(IReadOnlyList<CourierDto> couriers)
    {
        Couriers = couriers;
    }

    public IReadOnlyList<CourierDto> Couriers { get; }
}

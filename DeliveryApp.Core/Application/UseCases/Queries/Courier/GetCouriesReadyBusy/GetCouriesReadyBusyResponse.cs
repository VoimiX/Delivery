using DeliveryApp.Core.Application.UseCases.Queries.Courier.Dto;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.Courier.GetCouriesReadyBusy;

public class GetCouriesReadyBusyResponse : IRequest<GetCouriesReadyBusyResponse>
{
    public GetCouriesReadyBusyResponse(IReadOnlyList<CourierDto> couriers)
    {
        Couriers = couriers;
    }

    public IReadOnlyList<CourierDto> Couriers { get; }
}

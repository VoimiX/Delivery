using DeliveryApp.Core.Application.UseCases.Queries.Courier.Dto;
using static Api.Models.Courier;

namespace DeliveryApp.Api.Mappers;

public static class EnumMapper
{
    public static StatusEnum ConvertCourierStatus(CourierDto.CourierStatus status) => status switch
    {
        CourierDto.CourierStatus.NotAvailable => StatusEnum.NotAvailableEnum,
        CourierDto.CourierStatus.Ready => StatusEnum.ReadyEnum,
        CourierDto.CourierStatus.Busy => StatusEnum.BusyEnum,
        _ => throw new ArgumentOutOfRangeException(nameof(status), $"Not expected value: {status}"),
    };
}

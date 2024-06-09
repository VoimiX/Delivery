using Dapper;
using DeliveryApp.Core.Application.UseCases.Queries.Courier.Dto;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using MediatR;
using Npgsql;

namespace DeliveryApp.Core.Application.UseCases.Queries.Courier.GetCouriesReadyBusy;

public class GetCouriesReadyBusyHandler : IRequestHandler<GetCouriesReadyBusyQuery, GetCouriesReadyBusyResponse>
{
    private readonly string _connectionString;

    public GetCouriesReadyBusyHandler(string connectionString)
    {
        _connectionString = !string.IsNullOrWhiteSpace(connectionString)
            ? connectionString
            : throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    /// Получить готовых и занятых курьеров.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetCouriesReadyBusyResponse> Handle(GetCouriesReadyBusyQuery request, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(
            @"select id, name, location_x, location_y
                    FROM public.couriers where status=@status_busy or status=@status_ready",
            new { status_busy = (int)CourierStatus.Busy, status_ready = (int)CourierStatus.Ready}
            );

        return new GetCouriesReadyBusyResponse(MapCouriers(result));
    }

    private List<CourierDto> MapCouriers(IEnumerable<dynamic> result)
    {
        var orders = new List<CourierDto>();
        foreach (var item in result)
        {
            var order = new CourierDto(
                item.id,
                item.name,
                new Location(item.location_x, item.location_y) );
            orders.Add(order);
        }

        return orders;
    }
}

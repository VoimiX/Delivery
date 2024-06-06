using Dapper;
using DeliveryApp.Core.Application.UseCases.Queries.Courier.Dto;
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


    public async Task<GetCouriesReadyBusyResponse> Handle(GetCouriesReadyBusyQuery request, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(
            @"SELECT *
                    FROM public.orders where statu=@status");

        return new GetCouriesReadyBusyResponse(MapCouriers(result));

    }

    private List<CourierDto> MapCouriers(IEnumerable<dynamic> result)
    {
        var orders = new List<CourierDto>();
        foreach (var item in result)
        {
            var order = new CourierDto(item.id, item.name);
            orders.Add(order);
        }

        return orders;
    }
}

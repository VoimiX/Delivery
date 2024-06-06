using Dapper;
using DeliveryApp.Core.Application.UseCases.Queries.Order.Dto;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using MediatR;
using Npgsql;

namespace DeliveryApp.Core.Application.UseCases.Queries.Order.GetOrdersAssigned;

public class GetOrdersAssignedHandler : IRequestHandler<GetGetOrdersAssignedQuery, GetOrdersAssignedResponse>
{
    private readonly string _connectionString;

    public GetOrdersAssignedHandler(string connectionString)
    {
        _connectionString = !string.IsNullOrWhiteSpace(connectionString)
            ? connectionString
            : throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    /// Получаить все незавершенные заказы.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<GetOrdersAssignedResponse> Handle(GetGetOrdersAssignedQuery request, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(
            @"select id, location_x, location_y, weight
                    from public.orders o 
                    where status <> @status", new { status = (int)OrderStatus.Completed  });

        return new GetOrdersAssignedResponse(MapOrders(result));
    }

    private List<OrderDto> MapOrders(IEnumerable<dynamic> result)
    {
        var orders = new List<OrderDto>();
        foreach (var item in result)
        {
            var order = new OrderDto(
                item.id,
                new Location(item.location_x, item.location_y), new Weight(item.weight));

            orders.Add(order);
        }

        return orders;
    }
}

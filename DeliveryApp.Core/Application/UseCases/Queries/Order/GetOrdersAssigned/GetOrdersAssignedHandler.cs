using Dapper;
using DeliveryApp.Core.Application.UseCases.Queries.Order.Dto;
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

    public async Task<GetOrdersAssignedResponse> Handle(GetGetOrdersAssignedQuery request, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(
            @"SELECT *
                    FROM public.orders where status =@status");

        return new GetOrdersAssignedResponse(MapOrders(result));
    }

    private List<OrderDto> MapOrders(IEnumerable<dynamic> result)
    {
        var orders = new List<OrderDto>();
        foreach (var item in result)
        {
            var order = new OrderDto(item.id, new Location(item.x, item.y), new Weight(item.Weight));
            orders.Add(order);
        }

        return orders;
    }
}

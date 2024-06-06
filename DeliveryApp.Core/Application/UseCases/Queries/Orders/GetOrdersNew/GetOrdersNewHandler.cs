using Dapper;
using DeliveryApp.Core.Application.UseCases.Queries.Orders.Dto;
using DeliveryApp.Core.Domain.SharedKernel;
using MediatR;
using Npgsql;

namespace DeliveryApp.Core.Application.UseCases.Queries.Orders.GetOrdersNew;

public class GetOrdersNewHandler : IRequestHandler<GetGetOrdersNewQuery, GetOrdersNewResponse>
{
    private readonly string _connectionString;

    public GetOrdersNewHandler(string connectionString)
    {
        _connectionString = !string.IsNullOrWhiteSpace(connectionString)
            ? connectionString
            : throw new ArgumentNullException(nameof(connectionString));
    }


    public async Task<GetOrdersNewResponse> Handle(GetGetOrdersNewQuery request, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var result = await connection.QueryAsync<dynamic>(
            @"SELECT *
                    FROM public.orders where statu=@status");

        return new GetOrdersNewResponse(MapOrders(result));

    }

    private List<OrderDto> MapOrders(IEnumerable<dynamic> result)
    {
        var orders = new List<OrderDto>();
        foreach (var item in result)
        {
            var order = new OrderDto(item.id, new Location(item.x, item.y), new Weight(item.Weight), item.status);
            orders.Add(order);
        }

        return orders;
    }
}

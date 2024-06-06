using DeliveryApp.Core.Application.UseCases.Queries.Orders.Dto;

namespace DeliveryApp.Core.Application.UseCases.Queries.Orders.GetOrdersAssigned;

public class GetOrdersAssignedResponse
{
    public GetOrdersAssignedResponse(IReadOnlyList<OrderDto> orders)
    {
        Orders = orders;
    }

    public IReadOnlyList<OrderDto> Orders { get; }
}

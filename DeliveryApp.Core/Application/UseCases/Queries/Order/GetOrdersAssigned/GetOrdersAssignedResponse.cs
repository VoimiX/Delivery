using DeliveryApp.Core.Application.UseCases.Queries.Order.Dto;

namespace DeliveryApp.Core.Application.UseCases.Queries.Order.GetOrdersAssigned;

public class GetOrdersAssignedResponse
{
    public GetOrdersAssignedResponse(IReadOnlyList<OrderDto> orders)
    {
        Orders = orders;
    }

    public IReadOnlyList<OrderDto> Orders { get; }
}

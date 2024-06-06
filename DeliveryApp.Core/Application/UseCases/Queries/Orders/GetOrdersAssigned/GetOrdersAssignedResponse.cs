using DeliveryApp.Core.Application.UseCases.Queries.Orders.Dto;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.Orders.GetOrdersAssigned;

public class GetOrdersAssignedResponse : IRequest<GetOrdersAssignedResponse>
{
    public GetOrdersAssignedResponse(IReadOnlyList<OrderDto> orders)
    {
        Orders = orders;
    }

    public IReadOnlyList<OrderDto> Orders { get; }
}

using DeliveryApp.Core.Application.UseCases.Queries.Orders.Dto;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.Orders.GetOrdersNew;

public class GetOrdersNewResponse : IRequest<GetOrdersNewResponse>
{
    public GetOrdersNewResponse(IReadOnlyList<OrderDto> orders)
    {
        Orders = orders;
    }

    public IReadOnlyList<OrderDto> Orders { get; }
}

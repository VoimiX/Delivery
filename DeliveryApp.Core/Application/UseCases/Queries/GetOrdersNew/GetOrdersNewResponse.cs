using DeliveryApp.Core.Application.UseCases.Queries.GetOrdersNew.Dto;
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetOrdersNew;

public class GetOrdersNewResponse : IRequest<GetOrdersNewResponse>
{
    public GetOrdersNewResponse(IReadOnlyList<OrderDto> orders)
    {
        Orders = orders;
    }

    public IReadOnlyList<OrderDto> Orders { get; }
}

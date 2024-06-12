using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.EndWork;

public class EndWorkCommand : IRequest<EndWorkResponse>
{
    public Guid CourierId { get; }

    public EndWorkCommand(Guid courierId)
    {
        CourierId = courierId;
    }
}

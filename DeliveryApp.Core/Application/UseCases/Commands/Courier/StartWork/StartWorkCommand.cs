using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.Courier.StartWork;

public class StartWorkCommand : IRequest<StartWorkResponse>
{
    public Guid CourierId { get; }

    public StartWorkCommand(Guid courierId)
    {
        CourierId = courierId;
    }
}

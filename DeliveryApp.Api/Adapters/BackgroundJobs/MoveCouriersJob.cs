using DeliveryApp.Core.Application.UseCases.Commands.Courier.MoveToOrder;
using MediatR;
using Quartz;

namespace DeliveryApp.Api.Adapters.BackgroundJobs;

public class MoveCouriersJob : IJob
{
    private readonly IMediator _mediator;
    public MoveCouriersJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new MoveToOrderCommand());
    }
}

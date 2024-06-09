using DeliveryApp.Core.Application.UseCases.Commands.Courier.AssignOrder;
using MediatR;
using Quartz;

namespace DeliveryApp.Api.Adapters.BackgroundJobs;

public class AssignOrdersJob : IJob
{
    private readonly IMediator _mediator;

    public AssignOrdersJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new AssignOrderCommand());
    }
}

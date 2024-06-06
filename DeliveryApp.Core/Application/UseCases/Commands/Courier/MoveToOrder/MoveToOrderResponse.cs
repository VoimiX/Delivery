using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Application.UseCases.Commands.Couriers.MoveToOrder;

public class MoveToOrderResponse
{
    public MoveToOrderResponse(Location location)
    {
        Location = location;
    }

    public Location Location { get; }
}

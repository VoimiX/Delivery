using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Application.UseCases.Queries.Orders.Dto;

public class OrderDto
{
    public OrderDto(Guid id, Location location, Weight weight)
    {
        Id = id;
        Location = location;
        Weight = weight;
    }

    public Guid Id { get; }
    public Location Location { get; }
    public Weight Weight { get; }
}
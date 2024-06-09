
using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Application.UseCases.Queries.Courier.Dto;

public class CourierDto
{
    public CourierDto(Guid id, string name, Location location)
    {
        Id = id;
        Name = name;
        Location = location;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Location Location { get; }
}

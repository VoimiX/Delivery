
using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Application.UseCases.Queries.Courier.Dto;

public class CourierDto
{
    public CourierDto(Guid id, string name, Location location, CourierStatus status)
    {
        Id = id;
        Name = name;
        Location = location;
        Status = status;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Location Location { get; }
    public CourierStatus Status { get; }

    public enum CourierStatus
    {
        NotAvailable = 1,
        Ready = 2,
        Busy = 3
    }
}


using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetOrdersNew.Dto;

public class OrderDto
{
    public OrderDto(Guid id, Location location, Weight weight, int status)
    {
        Id = id;
        Location = location;
        Weight = weight;
        Status = (OrderStatusDto)status;
    }

    public Guid Id { get; }
    public Location Location { get; }
    public Weight Weight { get; }
    public OrderStatusDto Status { get; }
}

public enum OrderStatusDto
{
    Created = 1,
    Assigned = 2,
    Completed = 3
}

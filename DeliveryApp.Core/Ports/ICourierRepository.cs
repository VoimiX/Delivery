using DeliveryApp.Core.Domain.CourierAggregate;

namespace DeliveryApp.Core.Ports;

public interface ICourierRepository
{
    Task<Courier> AddCourier(Courier courier);
    Task UpdateCourier(Courier courier);
    Task<Courier> GetCourier(Guid id);
    Task<Courier[]> GetFreeCouriers();
    Task<Courier[]> GetAssignedCouriers();
}

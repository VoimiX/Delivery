using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Ports;

public interface IGeoServiceClient
{
    Task<Location> GetAddressLocation(string address);
}

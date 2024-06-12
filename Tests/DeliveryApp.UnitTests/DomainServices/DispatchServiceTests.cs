using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.Exceptions;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.DomainServices;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.DomainServices;

public class DispatchServiceTests
{
    [Fact]
    public async Task Test_dispatch_best_courier()
    {
        var courierCar = new Courier(Guid.NewGuid(), "courier1", Transport.Car);
        var courierPed = new Courier(Guid.NewGuid(), "courier2", Transport.Pedestrain);
        var courierScooter = new Courier(Guid.NewGuid(), "courier3", Transport.Scooter);

        courierCar.StartWork();
        courierPed.StartWork();
        courierScooter.StartWork();

        Order order = new Order(Guid.NewGuid(), new Location(2, 5), new Weight(5));

        var dispatchService = new DispatchService();
        var bestCourier = await dispatchService.Dispatch(order, new[] { courierCar, courierPed, courierScooter });

        bestCourier.Should().NotBeNull();
        bestCourier.Should().Be(courierCar);
        bestCourier.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public async Task Test_dispatch_best_courier_not_found_weight_exceeds()
    {        
        var courierPed = new Courier(Guid.NewGuid(), "courier2", Transport.Pedestrain);
        var courierScooter = new Courier(Guid.NewGuid(), "courier3", Transport.Scooter);
        courierPed.StartWork();
        courierScooter.StartWork();

        Order order = new Order(Guid.NewGuid(), new Location(2, 5), new Weight(100));

        var dispatchService = new DispatchService();

        var bestCourier = await dispatchService.Dispatch(order, new[] { courierPed, courierScooter });

       bestCourier.Should().BeNull();
    }
}

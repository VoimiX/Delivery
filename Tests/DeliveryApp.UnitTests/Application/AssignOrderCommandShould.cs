using DeliveryApp.Core.Application.UseCases.Commands.Courier.AssignOrder;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.DomainServices;
using DeliveryApp.Core.Ports;
using NSubstitute;
using Primitives;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DeliveryApp.UnitTests.Application;

public class AssignOrderCommandShould
{
    private readonly ICourierRepository _courierRepositoryMock;
    private readonly IOrderRepository _orderRepositoryMock;
    private readonly IUnitOfWork _unitOfWork;

    public AssignOrderCommandShould()
    {
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _courierRepositoryMock = Substitute.For<ICourierRepository>();
        _orderRepositoryMock = Substitute.For<IOrderRepository>();
    }

    [Fact]
    public async Task UpdateOrderAndSaveEntities()
    {
        //Arrange

        var orderId = Guid.NewGuid();

        var courierIdCar = Guid.NewGuid();
        var courierIdPed = Guid.NewGuid();

        var curierCar = new Courier(courierIdCar, "Petr Petrov", new Car(12, "Ford Ttransit"));
        curierCar.SetStatus(CourierStatus.Ready);
        _courierRepositoryMock.GetCourier(courierIdCar)
            .Returns(Task.FromResult(curierCar));

        var courierPed = new Courier(courierIdPed, "Petr Petrov", new Pedestrian(188, "Petr Petrov"));
        courierPed.SetStatus(CourierStatus.Ready);
        _courierRepositoryMock.GetCourier(courierIdPed)
            .Returns(Task.FromResult(courierPed));

        _orderRepositoryMock.GetOrder(Arg.Any<Guid>())
            .Returns(Task.FromResult(new Order(orderId, new Location(5, 8), new Weight(8))));

        _unitOfWork.SaveEntitiesAsync()
            .Returns(Task.FromResult(true));

        var dispatchService = new DispatchService();

        var command = new AssignOrderCommand(new[] { courierIdCar, courierIdPed }, orderId);
        var handler =
            new AssignOrderHandler(_unitOfWork, _courierRepositoryMock, _orderRepositoryMock, dispatchService);

        //Act
        await handler.Handle(command, CancellationToken.None);

        //Assert
        await _orderRepositoryMock.Received().UpdateOrder(Arg.Any<Order>());
        await _unitOfWork.Received().SaveEntitiesAsync(Arg.Any<CancellationToken>());
    }
}

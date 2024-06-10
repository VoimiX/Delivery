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

        var curierCar = new Courier(courierIdCar, "Petr Petrov", Transport.Car);
        curierCar.SetStatus(CourierStatus.Ready);

        var courierPed = new Courier(courierIdPed, "Petr Petrov", Transport.Pedestrain);
        courierPed.SetStatus(CourierStatus.Ready);

        _courierRepositoryMock.GetFreeCouriers().Returns([curierCar, courierPed]);

        _orderRepositoryMock.GetOrdersNew()
            .Returns(Task.FromResult(new[] { new Order(orderId, new Location(5, 8), new Weight(8)) }));

        _unitOfWork.SaveEntitiesAsync()
            .Returns(Task.FromResult(true));

        var dispatchService = new DispatchService();

        var command = new AssignOrderCommand();
        var handler =
            new AssignOrderHandler(_unitOfWork, _courierRepositoryMock, _orderRepositoryMock, dispatchService);

        //Act
        await handler.Handle(command, CancellationToken.None);

        //Assert
        await _orderRepositoryMock.Received().UpdateOrder(Arg.Any<Order>());
        await _unitOfWork.Received().SaveEntitiesAsync(Arg.Any<CancellationToken>());
    }
}

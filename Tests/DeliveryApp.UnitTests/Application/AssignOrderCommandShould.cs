using DeliveryApp.Core.Application.UseCases.Commands.Courier.AssignOrder;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
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
        var courierId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        _courierRepositoryMock.GetCourier(Arg.Any<Guid>())
            .Returns(Task.FromResult(new Courier(courierId, "Petr Petrov", new Car(12, "Ford Ttransit"))));

        _orderRepositoryMock.GetOrder(Arg.Any<Guid>())
            .Returns(Task.FromResult(new Order(orderId, new Location(5, 8), new Weight(10))));

        _unitOfWork.SaveEntitiesAsync()
            .Returns(Task.FromResult(true));

        var command = new AssignOrderCommand(courierId, orderId);
        var handler =
            new AssignOrderCommandHandler(_unitOfWork, _courierRepositoryMock, _orderRepositoryMock);

        //Act
        await handler.Handle(command, CancellationToken.None);

        //Assert
        await _orderRepositoryMock.Received().UpdateOrder(Arg.Any<Order>());
        await _unitOfWork.Received().SaveEntitiesAsync(Arg.Any<CancellationToken>());
    }
}

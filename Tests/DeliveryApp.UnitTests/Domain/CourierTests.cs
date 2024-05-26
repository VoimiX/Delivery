using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using System;
using Xunit;
using FluentAssertions;
using DeliveryApp.Core.Domain.Exceptions;

namespace DeliveryApp.UnitTests.Domain;

public class CourierTests
{
    [Fact]
    public void Test_courier_car_steps_distance_to_order()
    {
        Courier courier = new Courier(id: 7, "Vasya", new Car(id: 38, "Gazel"));        

        Order order = new Order(
            new Guid("11d34e42-5195-4dbc-a5a9-d7b45af19d78"),
            new Location(3, 4),
            new Weight(kilograms: 4)
            );        

        courier.StartWork();
        courier.Status.Should().Be(CourierStatus.Ready);

        courier.AssignOrder(order);
        courier.Order.Should().Be(order);
        courier.Order.Status.Should().Be(OrderStatus.Assigned);
        courier.Status.Should().Be(CourierStatus.Busy);        

        courier.StepsDistanceToOrder.Should().NotBeNull();
        courier.StepsDistanceToOrder.Value.Should().BeApproximately(
            (float)courier.Location.DistanceTo(order.Location) / courier.Transport.Speed, precision: 2);

        courier.MakeStepToOrder();
        courier.Status.Should().Be(CourierStatus.Busy);
        courier.Location.Should().Be(new Location(3, 3));

        courier.MakeStepToOrder();
        courier.Status.Should().Be(CourierStatus.Ready);
        courier.Order.Status.Should().Be(OrderStatus.Completed);
        courier.Location.Should().Be(new Location(3, 4));
    }

    [Fact]
    public void Test_courier_pedestrian_steps_distance_to_order()
    {
        Courier courier = new Courier(id: 7, "Vasya", new Pedestrian(id: 22, "Vasya"));

        Order order = new Order(
            new Guid("11d34e42-5195-4dbc-a5a9-d7b45af19d78"),
            new Location(3, 2),
            new Weight(kilograms: 1)
            );

        courier.StartWork();
        courier.Status.Should().Be(CourierStatus.Ready);

        courier.AssignOrder(order);
        courier.Order.Should().Be(order);
        courier.Order.Status.Should().Be(OrderStatus.Assigned);
        courier.Status.Should().Be(CourierStatus.Busy);

        courier.StepsDistanceToOrder.Should().NotBeNull();
        courier.StepsDistanceToOrder.Value.Should().BeApproximately(
            (float)courier.Location.DistanceTo(order.Location) / courier.Transport.Speed, precision: 2);

        courier.MakeStepToOrder();
        courier.Status.Should().Be(CourierStatus.Busy);
        courier.Location.Should().Be(new Location(2, 1));

        courier.MakeStepToOrder();
        courier.Status.Should().Be(CourierStatus.Busy);
        courier.Location.Should().Be(new Location(3, 1));

        courier.MakeStepToOrder();
        courier.Status.Should().Be(CourierStatus.Ready);
        courier.Order.Status.Should().Be(OrderStatus.Completed);
        courier.Location.Should().Be(new Location(3, 2));
    }

    [Fact]
    public void Test_fail_if_weight_exceeds()
    {
        Courier courier = new Courier(id: 7, "Vasya", new Pedestrian(id: 22, "Vasya"));

        Order order = new Order(
            new Guid("11d34e42-5195-4dbc-a5a9-d7b45af19d78"),
            new Location(3, 2),
            new Weight(kilograms: 10)
            );

        courier.StartWork();
        courier.Status.Should().Be(CourierStatus.Ready);
        
        var deliveryException = Assert.Throws<DeliveryException>(() => courier.AssignOrder(order));
        deliveryException.Message.Should().Be($"Транспорт курьера {courier.Transport} не позволяет по весу перевозить такой заказ.");
    }
}
